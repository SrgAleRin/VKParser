// <copyright file="PostImagesTable.cs" company="LANIT">
// 
//     Copyright (c) LAboratory of New Information Technologies. All rights reserved. 2018
// 
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseManager.Entities
{
    public class PostImagesTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PostItemId { get; set; }

        [ForeignKey("PostItemId")]
        public PostItemTable PostItem { get; set; }

        public string Image { get; set; }
    }
}