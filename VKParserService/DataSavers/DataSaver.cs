using System.Collections.Generic;
using System.Linq;
using DataBaseManager;
using DataBaseManager.Entities;
using Microsoft.EntityFrameworkCore;
using VkParser.SharedLib.Common;

namespace VKParserService.DataSavers
{
    internal class DataSaver
    {
        public void SaveToDB(List<PostItem> posts)
        {
            using(var db = VkDbContext.Connect())
            {
                db.Database.Migrate();

                foreach (var item in posts)
                {
                    if (db.PostItemTable.Any(i => i.PostId.Equals(item.Id))) continue;
                    PostItemTable main = new PostItemTable();
                    main.PostId = item.Id;
                    main.Author = item.Data.Author;
                    main.Text = item.Data.Text;
                    db.Add(main);

                    foreach (var link in item.Links.Items)
                    {
                        PostLinksTable links = new PostLinksTable();
                        links.PostItem = main;
                        links.Link = link;
                        db.Add(links);
                    }

                    foreach (var image in item.Images.ImageFiles)
                    {
                        PostImagesTable images = new PostImagesTable();
                        images.PostItem = main;
                        images.Image = image;
                        db.Add(images);
                    }

                    db.SaveChanges();
                }

            }
        }
    }
}
