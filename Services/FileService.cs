using Dapper;
using Npgsql;

namespace WebApp.Services;

public class FileService
{
    private string FilesDir => Path.Combine(Directory.GetCurrentDirectory(), "Files");
    private readonly NpgsqlConnection _npgsqlConnection;

    public FileService(NpgsqlConnection npgsqlConnection)
    {
        _npgsqlConnection = npgsqlConnection;
    }

    public async Task<int> UploadFileAsync(Stream stream, string fileName)
    {

        await using (var filestream = NewFile(FilesDir, fileName))
        {
            await stream.CopyToAsync(filestream);
        }
        return await SaveToDataBase(fileName);
    }

    private async Task<int> SaveToDataBase(String filename)
    {
        await _npgsqlConnection.OpenAsync();
        return await _npgsqlConnection.ExecuteScalarAsync<int>(@"
    insert into files(file_name)
    values (@file_name)
    returning id;
    ", new { file_name = filename });
    }

    private FileStream NewFile(string folderpath, string filename)
    {
        Directory.CreateDirectory(folderpath);
        return File.Create(Path.Combine(folderpath, filename));
    }

    public async Task<(FileStream file, string filename)> ReadFileAsync(int id)
    {
        var readname = await GetFileNameById(id);
        if (string.IsNullOrWhiteSpace(readname))
        {
            throw new Exception(" not found");
        }

        var file = File.Open(Path.Combine(FilesDir, readname), FileMode.Open, FileAccess.Read);
        return (file, readname);
    }

    private async Task<string> GetFileNameById(int id)
    {
        await _npgsqlConnection.OpenAsync();
        return await _npgsqlConnection.ExecuteScalarAsync<string>(@"
    select file_name from files where id=@id;
    ", new { id = id });
    }
}