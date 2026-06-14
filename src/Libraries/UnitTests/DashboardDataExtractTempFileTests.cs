using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using SenDev.Xaf.Dashboards.BusinessObjects;
using Xunit;

namespace UnitTests
{
	public class DashboardDataExtractTempFileTests
	{
		private static readonly Regex HashFormat = new Regex("^[0-9a-f]{64}$");

		private static byte[] Bytes(string text) => Encoding.UTF8.GetBytes(text);

		private static string GetExtractFolder(DashboardDataExtract extract) =>
			Path.Combine(Path.GetTempPath(), "XafDashboards", extract.Oid.ToString());

		private static void RunWithExtract(Action<XpoInMemoryXafApplication, DashboardDataExtract> test)
		{
			using (var application = XpoInMemoryXafApplication.CreateInstance())
			using (var objectSpace = application.CreateObjectSpace())
			{
				var extract = objectSpace.CreateObject<DashboardDataExtract>();
				objectSpace.CommitChanges();

				string extractFolder = GetExtractFolder(extract);
				try
				{
					test(application, extract);
				}
				finally
				{
					if (Directory.Exists(extractFolder))
						Directory.Delete(extractFolder, true);
				}
			}
		}

		[Fact]
		public void EnsureTempFileCreated_CreatesFileWithExpectedPath()
		{
			RunWithExtract((application, extract) =>
			{
				byte[] data = Bytes("hello world");
				extract.ExtractData = data;

				string expectedHash = DashboardDataExtract.ComputeHash(data);
				string expectedPath = Path.Combine(
					GetExtractFolder(extract), expectedHash, "ExtractData.dat");

				string path = extract.EnsureTempFileCreated(application);

				Assert.Equal(expectedPath, path);
				Assert.True(File.Exists(path));
				Assert.Equal(data, File.ReadAllBytes(path));
			});
		}

		[Fact]
		public void EnsureTempFileCreated_ReturnsNull_WhenNoData()
		{
			RunWithExtract((application, extract) =>
			{
				Assert.Null(extract.EnsureTempFileCreated(application));
			});
		}

		[Fact]
		public void EnsureTempFileCreated_ReusesExistingFile()
		{
			RunWithExtract((application, extract) =>
			{
				extract.ExtractData = Bytes("cached content");

				string path = extract.EnsureTempFileCreated(application);

				// Overwrite the cached file. Because the hash (and therefore the path) is
				// unchanged, a second call must not recreate/overwrite the file.
				byte[] sentinel = Bytes("manually modified");
				File.WriteAllBytes(path, sentinel);

				string secondPath = extract.EnsureTempFileCreated(application);

				Assert.Equal(path, secondPath);
				Assert.Equal(sentinel, File.ReadAllBytes(secondPath));
			});
		}

		[Fact]
		public void EnsureTempFileCreated_UsesPersistentHashForFolderName()
		{
			RunWithExtract((application, extract) =>
			{
				extract.ExtractData = Bytes("payload");
				string presetHash = new string('a', 64);
				extract.Hash = presetHash;

				string path = extract.EnsureTempFileCreated(application);

				string expectedPath = Path.Combine(
					GetExtractFolder(extract), presetHash, "ExtractData.dat");
				Assert.Equal(expectedPath, path);
			});
		}

		[Fact]
		public void EnsureTempFileCreated_DeletesOldHashFolder_WhenDataChanges()
		{
			RunWithExtract((application, extract) =>
			{
				extract.ExtractData = Bytes("first version");
				string firstPath = extract.EnsureTempFileCreated(application);
				string firstFolder = Path.GetDirectoryName(firstPath);

				// Change the data; with no persistent hash set the hash is recomputed,
				// producing a new folder and triggering cleanup of the old one.
				extract.ExtractData = Bytes("second version");
				string secondPath = extract.EnsureTempFileCreated(application);
				string secondFolder = Path.GetDirectoryName(secondPath);

				Assert.NotEqual(firstFolder, secondFolder);
				Assert.False(Directory.Exists(firstFolder), "Old hash folder should be deleted.");
				Assert.True(File.Exists(secondPath));
			});
		}

		[Fact]
		public void EnsureTempFileCreated_DoesNotDeleteFoldersWithNonHashName()
		{
			RunWithExtract((application, extract) =>
			{
				extract.ExtractData = Bytes("version a");
				extract.EnsureTempFileCreated(application);

				// Create a sibling folder whose name is not a valid hash. It must survive the
				// cleanup that happens when a new hash folder is created.
				string foreignFolder = Path.Combine(GetExtractFolder(extract), "do-not-delete-me");
				Directory.CreateDirectory(foreignFolder);
				string foreignFile = Path.Combine(foreignFolder, "important.txt");
				File.WriteAllText(foreignFile, "keep");

				extract.ExtractData = Bytes("version b");
				extract.EnsureTempFileCreated(application);

				Assert.True(Directory.Exists(foreignFolder), "Non-hash folder must not be deleted.");
				Assert.True(File.Exists(foreignFile));
			});
		}

		[Fact]
		public void ComputeHash_ProducesLowercaseHex_AndIsDeterministic()
		{
			byte[] data = Bytes("repeatable");

			string hash1 = DashboardDataExtract.ComputeHash(data);
			string hash2 = DashboardDataExtract.ComputeHash(Bytes("repeatable"));

			Assert.Matches(HashFormat, hash1);
			Assert.Equal(hash1, hash2);
		}

		[Fact]
		public void ComputeHash_DiffersForDifferentData()
		{
			Assert.NotEqual(
				DashboardDataExtract.ComputeHash(Bytes("a")),
				DashboardDataExtract.ComputeHash(Bytes("b")));
		}

		[Fact]
		public void ComputeHash_ReturnsNull_ForNull()
		{
			Assert.Null(DashboardDataExtract.ComputeHash(null));
		}
	}
}
