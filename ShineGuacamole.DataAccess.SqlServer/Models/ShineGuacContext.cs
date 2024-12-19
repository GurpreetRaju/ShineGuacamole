#region Copyright
//
// Copyright 2024 Gurpreet Raju
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
#endregion

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShineGuacamole.DataAccess.SqlServer.Models;

/// <summary>
/// The database context.
/// </summary>
public class ShineGuacContext : DbContext
{
    #region Constructor

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="options"></param>
    public ShineGuacContext(DbContextOptions<ShineGuacContext> options)
        : base(options)
    {
    }

    #endregion


    #region Tables

    /// <summary>
    /// The connections table.
    /// </summary>
    public DbSet<Connection> Connections { get; set; }

    #endregion


    #region Methods

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Connection>(entity =>
        {
            entity.Property(e => e.ConnectionId)
                .HasDefaultValueSql("(newid())");
            
            entity.Property(e => e.Image)
                .HasColumnType("image");
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(e => e.CreationDateTime)
                .IsRequired();

            entity.Property(e => e.Owner)
                .IsRequired()
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.Property(e => e.Properties)
                .IsRequired();

            entity.Property(e => e.Tags)
                .HasMaxLength(100);
        });
    }

    #endregion
}