using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using MyAamdhani.Helper;
using MyAamdhani.Models;

namespace MyAamdhani.HelperClasses
{
    public class RepoHelper : RepoBase
    {
        public IEnumerable<CartItems> GetCartItems( int userId)
        {
            IEnumerable<CartItems> list = new List<CartItems>();
            try
            {
                var query = $@"select ci.Id [CartItemId],ci.ProductId,ci.Quantity,ci.[Json],ci.UnitPrice,ci.UserId,ci.DateModified,ci.DateAdded , 
                                p.Name as ProductName,p.UniqueId as ProductUniqueKey, Concat(ud.FirstName,' ',ud.lastname) as [CustomerName],(select top 1 icr.[image] from tbl_ICRWithProduct icr where 
                                icr.ProductId=p.Id) as [CoverImage]

                                from cartitem ci 
                                Left join Products p on p.id= ci.productId
                                left join [Users] u on u.Id=ci.userid
                                left join [UserDetail] ud on ud.UserId=u.Id
                                where ci.userid={userId} and ci.isactive=1 ";
                Con.Open();
                list = Con.Query<CartItems>(query, commandTimeout: 0);
            }
            catch (Exception e)
            {
                Log.Debug(e);
            }
            finally
            {
                Con.Close();
            }
            return list;
        }
        public ProductDetails GetProductDetails(int? productId,string productUniqueId="")
        {
            ProductDetails productDetails = new ProductDetails();
            try
            {
                var queryPart = string.Empty;

                if (!ReferenceEquals(productId, null))
                {
                    queryPart = $"and  p.Id={productId}";
                }
                else if (!string.IsNullOrEmpty(productUniqueId))
                {
                    queryPart = $"and p.UniqueId='{productUniqueId}'";
                }
                else
                {
                    queryPart = "and  p.Id = 0";
                }

                var query = $@"select p.Id as ProductId,  p.Name as ProductName,(select top 1 icr.[image] from tbl_ICRWithProduct icr where icr.ProductId=p.Id) as [CoverImage],
                                p.Description as ProductDescription,p.PricePerPiece as PiecePrice,p.MRPPerPiece as MRPPrice,p.SKUId,p.HSNCode,p.Type as ProductType,
                                p.DateAdded,p.CategoryId,ct.CategoryName,p.SubCategoryId,sct.SubCategoryName,p.UniqueId as ProductUniqueKey,
                                f.Fabric_Name as Fabric,cs.Style_Name as ClotheStyle,pt.Pattern_Name as PatternName,sb.SareeBorder_Name as SareeBorder
                                from Products p
                                Left join tbl_Fabric f on f.Fabric_Id= p.FabricId
                                Left join tbl_ClothStyle cs on cs.Style_Id= p.StyleId
                                Left join tbl_Pattern pt on pt.Pattern_Id= p.PatternId
                                Left join tbl_SareeBorder sb on sb.SareeBorder_Id= p.SareeBorderId
                                Left join Category ct on ct.CategoryId= p.CategoryId
                                Left join SubCategory sct on sct.SubCategoryId= p.SubCategoryId
                                
                                
                                where p.IsActive=1 {queryPart} ";
                Con.Open();
                productDetails = Con.Query<ProductDetails>(query, commandTimeout: 0).FirstOrDefault();
            }
            catch (Exception e)
            {
                Log.Debug(e);
            }
            finally
            {
                Con.Close();
            }
            return productDetails;
        }

    }
}