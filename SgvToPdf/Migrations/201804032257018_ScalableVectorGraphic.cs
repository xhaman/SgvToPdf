namespace SgvToPdf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScalableVectorGraphic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScalableVectorGraphics",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        SgvSpecs = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ScalableVectorGraphics");
        }
    }
}
