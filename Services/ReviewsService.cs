using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class ReviewService
    {
        private readonly IDatabaseService _databaseService;

        public ReviewService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Review> GetAllReviews()
        {
            return _databaseService.Query<Review>(
                "SELECT * FROM Reviews WHERE Deleted IS NULL"
            );
        }

        public IEnumerable<Review> GetReviewsByProductId(int productId)
        {
            return _databaseService.Query<Review>(
                "SELECT * FROM Reviews WHERE ProductID = @ProductID AND Deleted IS NULL",
                new { ProductID = productId }
            );
        }

        public void AddReview(Review review)
        {
            _databaseService.Execute(
                @"INSERT INTO Reviews (ProductID, UserID, Rating, Comment, ReviewDate, Created, Modified, Deleted) 
                  VALUES (@ProductID, @UserID, @Rating, @Comment, @ReviewDate, @Created, NULL, NULL)",
                new
                {
                    review.ProductID,
                    review.UserID,
                    review.Rating,
                    review.Comment,
                    review.ReviewDate,
                    Created = DateTime.UtcNow // Imposta Created come la data e ora correnti
                }
            );
        }

        public void UpdateReview(Review review)
        {
            _databaseService.Execute(
                @"UPDATE Reviews 
                  SET ProductID = @ProductID, UserID = @UserID, Rating = @Rating, 
                      Comment = @Comment, ReviewDate = @ReviewDate, Modified = @Modified 
                  WHERE ReviewID = @ReviewID AND Deleted IS NULL",
                new
                {
                    review.ProductID,
                    review.UserID,
                    review.Rating,
                    review.Comment,
                    review.ReviewDate,
                    Modified = DateTime.UtcNow, // Imposta Modified come la data e ora correnti
                    review.ReviewID
                }
            );
        }

        public void DeleteReview(int reviewId)
        {
            _databaseService.Execute(
                @"UPDATE Reviews 
                  SET Deleted = @Deleted 
                  WHERE ReviewID = @ReviewID",
                new
                {
                    Deleted = DateTime.UtcNow, // Imposta Deleted come la data e ora correnti
                    ReviewID = reviewId
                }
            );
        }
    }
}
