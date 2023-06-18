namespace FinalProject_340.Models
{
    public class new_list_tracks
    {
        public List<IFormFile> trackFiles { get; set; }
        public IFormFile albumArt { get; set; }
        public void saveFiles()
        {
            foreach (IFormFile file in trackFiles)
            {
                _n_track track = new _n_track()
                {
                    formFile = file,
                    albumArt = this.albumArt,
                };
                track.setProps();
                if (track.isValid()) track.saveTrack();
            }
        }
    }
}
