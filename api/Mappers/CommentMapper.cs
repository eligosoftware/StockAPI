using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
         public static CommentDto ToCommentDto(this Comment commentModel){
            return new CommentDto{
                    Id = commentModel.Id,
                    Title = commentModel.Title,
                    Content = commentModel.Content,
                    CreatedOn = commentModel.CreatedOn,
                    CreatedBy = commentModel.AppUser.UserName,
                    StockId = commentModel.StockId
            };
        }

        public static Comment ToCommentFromCreateDTO(this CreateCommentDto createComment, int _stockId){
            return new Comment{
                    
                    Title = createComment.Title,
                    Content = createComment.Content,
                    StockId = _stockId
        };}

        public static Comment ToCommentFromUpdateDTO(this UpdateCommentRequestDto updateComment){
            return new Comment{
                    
                    Title = updateComment.Title,
                    Content = updateComment.Content
        };
    
        }
    }
}