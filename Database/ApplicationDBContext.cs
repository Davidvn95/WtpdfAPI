using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using WebToPdf.Models;

namespace WebToPdf.Database
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<ZipModel> Zip { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)   
        {
            try
            {
                var dbCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (dbCreator != null)
                {
                    if(!dbCreator.CanConnect())
                        dbCreator.Create();
                    
                    if(!dbCreator.HasTables())
                        dbCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<ZipModel> Get(int id)
        {
            try
            {
            return await Zip.FirstAsync(x => x.Id == id);
                
            }
            catch (System.Exception)
            {
                throw new Exception($"El documento con Id {id} no existe");
            }
        }


        public async Task<ZipModel[]> GetAll()
        {
            var zipList = await Zip.ToListAsync();
            return zipList.ToArray();
        }

        public async Task<ZipModel> Add(CreateZipDB entity)
        {
            ZipModel newEntity = new(){
                Id = null,
                Name = entity.Name,
                Content = entity.Content,
            };

            EntityEntry<ZipModel> response = await Zip.AddAsync(newEntity);
            await SaveChangesAsync();
            return await Get(response.Entity.Id ?? throw new Exception("No se ha podido guardar"));
        }

        public async Task<bool> Delete(int id)
        {
            var element = await Get(id);
            if(element == null)
                return false;
            
            Zip.Remove(element);
            SaveChanges();
            return true;
        }
    }
}