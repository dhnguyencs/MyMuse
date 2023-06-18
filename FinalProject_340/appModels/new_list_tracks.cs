namespace FinalProject_340.Models
{
    public class new_list_tracks
    {
        public List<IFormFile> songFiles { get; set; }
        public IFormFile albumArt { get; set; }
        public void saveFiles()
        {
            foreach (IFormFile file in songFiles)
            {
                _n_song song = new _n_song()
                {
                    formFile = file,
                    albumArt = this.albumArt,
                };
                song.setProps();
                if (song.isValid()) song.saveTrack();
            }
        }
    }
}
