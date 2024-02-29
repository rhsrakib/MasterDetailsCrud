﻿namespace MasterDetailsCrud.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SaleDetails",
                c => new
                    {
                        SaleDetailId = c.Int(nullable: false, identity: true),
                        SaleId = c.Int(),
                        ProductName = c.String(),
                        UnitPrice = c.Int(),
                        Quantity = c.Int(),
                        TotalPrice = c.Int(),
                    })
                .PrimaryKey(t => t.SaleDetailId)
                .ForeignKey("dbo.SaleMasters", t => t.SaleId)
                .Index(t => t.SaleId);
            
            CreateTable(
                "dbo.SaleMasters",
                c => new
                    {
                        SaleId = c.Int(nullable: false, identity: true),
                        CreateDate = c.DateTime(),
                        CustomerName = c.String(),
                        CustomerAddress = c.String(),
                        Gender = c.String(),
                        Photo = c.String(),
                        ProductType = c.String(),
                    })
                .PrimaryKey(t => t.SaleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleDetails", "SaleId", "dbo.SaleMasters");
            DropIndex("dbo.SaleDetails", new[] { "SaleId" });
            DropTable("dbo.SaleMasters");
            DropTable("dbo.SaleDetails");
        }
    }
}
