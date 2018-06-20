// <copyright file="Consts.cs" company="LANIT">
// 
//     Copyright (c) LAboratory of New Information Technologies. All rights reserved. 2018
// 
// </copyright>

using System;
using System.IO;

namespace SharedLib
{
    public static class Consts
    {
        public const string FileDirectory = @"f:\VKParse\";
        private const string MainDataFileName = "PostMainData.json";
        private const string LinksFileName = "PostLinks.json";
        private const string ImagesFileName = "PostImages.json";
        private const string FullPostFileName = "FullPost.json";
        public static string MainDataFilePath = Path.Combine(FileDirectory, MainDataFileName);
        public static string LinksFilePath = Path.Combine(FileDirectory, LinksFileName);
        public static string ImagesFilePath = Path.Combine(FileDirectory, ImagesFileName);
        public static string FullPostFilePath = Path.Combine(FileDirectory, FullPostFileName);
        public static TimeSpan DefaultWaitTime = TimeSpan.FromSeconds(10);
    }
}