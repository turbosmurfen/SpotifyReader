using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Numerics;
namespace SpotifyReader
{
    class Program
    {
        static void Main(string[] args)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Volatile Environment"))
            {
                if (key != null)
                {
                    Object obj = key.GetValue("LOCALAPPDATA");
                    if (obj != null)
                    {
                        string packPath = $"{obj}\\Packages";
                        string[] dirs = Directory.GetDirectories(packPath, "SpotifyAB.SpotifyMusic_*", SearchOption.TopDirectoryOnly);
                        if (dirs.Length == 1)
                        {
                            string newpath = $"{dirs[0]}\\LocalState\\Spotify\\Users\\";
                            string[] dir = Directory.GetDirectories(newpath, "*-user", SearchOption.TopDirectoryOnly);
                            if (dir.Length == 1)
                            {
                                Console.WriteLine(dir[0]);
                                string file = $"{dir[0]}\\context_player_state_restore";
                                if (File.Exists(file))
                                {
                                    string data = File.ReadAllText(file);
                                    Rootobject userdata = JsonConvert.DeserializeObject<Rootobject>(data.Substring(14));

                                    string duration = UnixToDate(userdata.player_model.session_queue.active.playback_state.duration);

                                    bool shuffle = userdata.player_model.options.shuffling_context;
                                    bool repeat = userdata.player_model.options.repeating_context;
                                    bool repeatTrack = userdata.player_model.options.repeating_track;

                                    string albumTitle = userdata.player_model.session_queue.active.track.metadata.overrides.album_title;

                                    string albumUri = userdata.player_model.session_queue.active.track.metadata.overrides.album_uri;
                                    string artistUri = userdata.player_model.session_queue.active.track.metadata.overrides.artist_uri;
                                    string trackUri = userdata.player_model.session_queue.active.track.uri;
                                    string uid = userdata.player_model.session_queue.active.track.uid;

                                    bool paused = userdata.player_model.session_queue.active.is_paused;
                                    bool playing = userdata.player_model.session_queue.active.is_playing;



                                    Console.WriteLine($"Shuffle: {shuffle}\nRepeat: {repeat}\nRepeat Track: {repeatTrack}\n\nPaused: {paused}\nPlaying: {playing}\n\nAlbum Title: {albumTitle}\nAlbum Uri: {albumUri}\nArtist Uri: {artistUri}\nTrack Uri: {trackUri}\n\nDuration: {duration}\n");

                                }
                            }
                        }
                    }
                }

            }

            Console.ReadKey();
        }
        public static string UnixToDate(long epoch)
        {
            DateTimeOffset dto = DateTimeOffset.FromUnixTimeMilliseconds(epoch);
            return dto.ToString("mm:ss");
        }

    }

    /*public class Rootobject
    {
        public int version { get; set; }
        public string version_suffix { get; set; }
        public Player_Model player_model { get; set; }
    }

    public class Player_Model
    {
        public Options options { get; set; }
        public Configuration configuration { get; set; }
        public Session_Queue session_queue { get; set; }
    }

    public class Options
    {
        public bool shuffling_context { get; set; }
        public bool repeating_context { get; set; }
        public bool repeating_track { get; set; }
    }

    public class Configuration
    {
        public bool audioautomix { get; set; }
        public int audioepisodespeed { get; set; }
        public object[] playerbannedalbums { get; set; }
        public object[] playerbannedartists { get; set; }
        public PlayerBannedContext_Tracks playerbannedcontext_tracks { get; set; }
        public object[] playerbannedtracks { get; set; }
        public bool playerfilter_age_restricted_content { get; set; }
        public bool playerfilter_explicit_content { get; set; }
        public string playerlicense { get; set; }
        public string videosubtitles { get; set; }
        public bool videosubtitles_cc { get; set; }
    }

    public class PlayerBannedContext_Tracks
    {
    }

    public class Session_Queue
    {
        public object[] pushed { get; set; }
        public Active active { get; set; }
    }

    public class Active
    {
        public Prepare_Play_Options prepare_play_options { get; set; }
        public Playback_State playback_state { get; set; }
        public Track track { get; set; }
    }

    public class Prepare_Play_Options
    {
        public bool always_play_something { get; set; }
        public Skip_To skip_to { get; set; }
        public bool initially_paused { get; set; }
        public bool system_initiated { get; set; }
        public Player_Options_Override player_options_override { get; set; }
        public Suppressions suppressions { get; set; }
        public string prefetch_level { get; set; }
        public string session_id { get; set; }
        public string audio_stream { get; set; }
        public Configuration_Override configuration_override { get; set; }
    }

    public class Skip_To
    {
        public string track_uid { get; set; }
        public string track_uri { get; set; }
    }

    public class Player_Options_Override
    {
    }

    public class Suppressions
    {
        public object[] providers { get; set; }
    }

    public class Configuration_Override
    {
    }

    public class Playback_State
    {
        public long timestamp { get; set; }
        public int position { get; set; }
        public int duration { get; set; }
        public bool is_buffering { get; set; }
        public Playback_Quality playback_quality { get; set; }
        public int playback_speed { get; set; }
    }

    public class Playback_Quality
    {
        public string bitrate_level { get; set; }
    }

    public class Track
    {
        public string uid { get; set; }
        public string uri { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Metadata
    {
        public Original original { get; set; }
        public Overrides overrides { get; set; }
    }

    public class Original
    {
    }

    public class Overrides
    {
        public string actionsskipping_next_past_track { get; set; }
        public string actionsskipping_prev_past_track { get; set; }
        public string album_title { get; set; }
        public string album_uri { get; set; }
        public string artist_uri { get; set; }
        public string context_uri { get; set; }
        public string entity_uri { get; set; }
        public string image_large_url { get; set; }
        public string image_small_url { get; set; }
        public string image_url { get; set; }
        public string image_xlarge_url { get; set; }
        public string iteration { get; set; }
        public string track_player { get; set; }
    }*/



    public class Rootobject
    {
        public int version { get; set; }
        public string version_suffix { get; set; }
        public Player_Model player_model { get; set; }
    }

    public class Player_Model
    {
        public Options options { get; set; }
        public Configuration configuration { get; set; }
        public Session_Queue session_queue { get; set; }
    }

    public class Options
    {
        public bool shuffling_context { get; set; }
        public bool repeating_context { get; set; }
        public bool repeating_track { get; set; }
    }

    public class Configuration
    {
        public bool audioautomix { get; set; }
        public int audioepisodespeed { get; set; }
        public object[] playerbannedalbums { get; set; }
        public object[] playerbannedartists { get; set; }
        public PlayerBannedContext_Tracks playerbannedcontext_tracks { get; set; }
        public object[] playerbannedtracks { get; set; }
        public bool playerfilter_age_restricted_content { get; set; }
        public bool playerfilter_explicit_content { get; set; }
        public bool playerhifi_addon { get; set; }
        public string playerlicense { get; set; }
        public string videosubtitles { get; set; }
        public bool videosubtitles_cc { get; set; }
    }

    public class PlayerBannedContext_Tracks
    {
    }

    public class Session_Queue
    {
        public object[] pushed { get; set; }
        public Active active { get; set; }
    }

    public class Active
    {
        public Prepare_Play_Options prepare_play_options { get; set; }
        public Playback_State playback_state { get; set; }
        public Track track { get; set; }
        public Next_Command_Logging_Params next_command_logging_params { get; set; }
        public Curr_Command_Logging_Params curr_command_logging_params { get; set; }
        public Play_Origin play_origin { get; set; }
        public bool is_playing { get; set; }
        public bool is_paused { get; set; }
        public bool is_system_initiated { get; set; }
        public bool is_finished { get; set; }
        public Options1 options { get; set; }
       // public BigInteger(playback_seed) { get; set; }
        public int num_advances { get; set; }
        public bool did_skip_prev { get; set; }
        public string license { get; set; }
        public Rules rules { get; set; }
    }

    public class Prepare_Play_Options
    {
        public bool always_play_something { get; set; }
        public Skip_To skip_to { get; set; }
        public bool initially_paused { get; set; }
        public bool system_initiated { get; set; }
        public Player_Options_Override player_options_override { get; set; }
        public Suppressions suppressions { get; set; }
        public string prefetch_level { get; set; }
        public string session_id { get; set; }
        public string audio_stream { get; set; }
        public Configuration_Override configuration_override { get; set; }
    }

    public class Skip_To
    {
        public string track_uid { get; set; }
        public string track_uri { get; set; }
        public int track_index { get; set; }
    }

    public class Player_Options_Override
    {
    }

    public class Suppressions
    {
        public object[] providers { get; set; }
    }

    public class Configuration_Override
    {
    }

    public class Playback_State
    {
        public long timestamp { get; set; }
        public int position { get; set; }
        public int duration { get; set; }
        public bool is_buffering { get; set; }
        public Playback_Quality playback_quality { get; set; }
        public int playback_speed { get; set; }
    }

    public class Playback_Quality
    {
        public string bitrate_level { get; set; }
        public string strategy { get; set; }
        public string target_bitrate_level { get; set; }
        public bool target_bitrate_available { get; set; }
        public string hifi_status { get; set; }
    }

    public class Track
    {
        public string uid { get; set; }
        public string uri { get; set; }
        public Metadata metadata { get; set; }
        public string provider { get; set; }
        public Internal_Metadata internal_metadata { get; set; }
    }

    public class Metadata
    {
        public Original original { get; set; }
        public Overrides overrides { get; set; }
    }

    public class Original
    {
    }

    public class Overrides
    {
        public string actionsskipping_next_past_track { get; set; }
        public string actionsskipping_prev_past_track { get; set; }
        public string album_title { get; set; }
        public string album_uri { get; set; }
        public string artist_uri { get; set; }
        public string context_uri { get; set; }
        public string entity_uri { get; set; }
        public string image_large_url { get; set; }
        public string image_small_url { get; set; }
        public string image_url { get; set; }
        public string image_xlarge_url { get; set; }
        public string iteration { get; set; }
        public string track_player { get; set; }
    }

    public class Internal_Metadata
    {
        public string is_audio_offline_available { get; set; }
    }

    public class Next_Command_Logging_Params
    {
        public object[] page_instance_ids { get; set; }
        public object[] interaction_ids { get; set; }
    }

    public class Curr_Command_Logging_Params
    {
        public object[] page_instance_ids { get; set; }
        public object[] interaction_ids { get; set; }
    }

    public class Play_Origin
    {
        public string feature_identifier { get; set; }
        public string feature_version { get; set; }
        public string view_uri { get; set; }
        public string external_referrer { get; set; }
        public string referrer_identifier { get; set; }
        public string device_identifier { get; set; }
        public object[] feature_classes { get; set; }
    }

    public class Options1
    {
        public bool shuffling_context { get; set; }
        public bool repeating_context { get; set; }
        public bool repeating_track { get; set; }
    }

    public class Rules
    {
        public object ad { get; set; }
        public object ad_playback { get; set; }
        public Analytics analytics { get; set; }
        public object auto_resume { get; set; }
        public Automix automix { get; set; }
        public object availability_rules { get; set; }
        public Behavior_Metadata behavior_metadata { get; set; }
        public Circuit_Breaker circuit_breaker { get; set; }
        public object context_mdata { get; set; }
        public object decorator { get; set; }
        public Explicit_Request explicit_request { get; set; }
    }

    public class Analytics
    {
        public object analyticslog_start_context { get; set; }
        public object analyticslog_start_track { get; set; }
    }

    public class Automix
    {
        public bool automix { get; set; }
        public string current_track_uri { get; set; }
    }

    public class Behavior_Metadata
    {
        public object[] page_instance_ids { get; set; }
        public object[] interaction_ids { get; set; }
    }

    public class Circuit_Breaker
    {
        public object[] discarded_track_uids { get; set; }
        public int num_errored_tracks { get; set; }
        public bool context_track_played { get; set; }
    }

    public class Explicit_Request
    {
        public bool always_play_something { get; set; }
    }
}
