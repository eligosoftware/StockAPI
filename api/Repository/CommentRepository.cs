using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {

        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comment
            .Include(a=> a.AppUser)
            .ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            // var comment = await _com
            return await _context.Comment
            .Include(a=> a.AppUser)
            .FirstOrDefaultAsync(c=> c.Id ==id);
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comment.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var existingComment = await _context.Comment.FirstOrDefaultAsync(x => x.Id == id);
            if(existingComment == null){
                return null;
            }

            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;

            await _context.SaveChangesAsync();
            return existingComment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _context.Comment.FirstOrDefaultAsync(x => x.Id == id);
            if(commentModel == null){
                return null;
            }

             _context.Comment.Remove(commentModel);

            await _context.SaveChangesAsync();
            return commentModel;
        }
    }
}