// <copyright file="PostItem.cs" company="LANIT">
// 
//     Copyright (c) LAboratory of New Information Technologies. All rights reserved. 2018
// 
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseManager.Entities
{
    public class PostItemTable
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string PostId { get; set; }

        public string Text { get; set; }
        public string Author { get; set; }

        public List<PostLinksTable> PostLinks { get; set; }

        public List<PostImagesTable> PostImages { get; set; }
    }
}