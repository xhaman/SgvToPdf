namespace SgvToPdf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TitleLengthSgv : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ScalableVectorGraphics", "Title", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.ScalableVectorGraphics", "SgvSpecs", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ScalableVectorGraphics", "SgvSpecs", c => c.String());
            AlterColumn("dbo.ScalableVectorGraphics", "Title", c => c.String());
        }
    }
}
