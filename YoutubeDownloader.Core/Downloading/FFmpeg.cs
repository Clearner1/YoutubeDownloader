﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YoutubeDownloader.Core.Downloading;

public static class FFmpeg
{
    private static string CliFileName { get; } =
        OperatingSystem.IsWindows() ? "ffmpeg.exe" : "ffmpeg";

    public static string? TryGetCliFilePath()
    {
        static IEnumerable<string> GetProbeDirectoryPaths()
        {
            yield return AppContext.BaseDirectory;
            yield return Directory.GetCurrentDirectory();

            // User PATH environment variable
            if (Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User)?.Split(Path.PathSeparator) is { } userPaths)
            {
                foreach (var path in userPaths)
                    yield return path;
            }

            // System PATH environment variable
            if (Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine)?.Split(Path.PathSeparator) is { } systemPaths)
            {
                foreach (var path in systemPaths)
                    yield return path;
            }
        }

        return GetProbeDirectoryPaths()
            .Select(dirPath => Path.Combine(dirPath, CliFileName))
            .FirstOrDefault(File.Exists);
    }

    public static bool IsBundled() =>
        File.Exists(Path.Combine(AppContext.BaseDirectory, CliFileName));

    public static bool IsAvailable() => !string.IsNullOrWhiteSpace(TryGetCliFilePath());
}