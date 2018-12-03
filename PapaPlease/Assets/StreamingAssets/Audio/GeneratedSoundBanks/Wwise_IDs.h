/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAY_AI_CLEANBIN = 3636964221U;
        static const AkUniqueID PLAY_AI_COOKING = 1056168201U;
        static const AkUniqueID PLAY_AI_DISHES = 2890421503U;
        static const AkUniqueID PLAY_AI_PEEING = 1614196089U;
        static const AkUniqueID PLAY_AI_PEEINGFLOOR = 636593309U;
        static const AkUniqueID PLAY_AI_PLAYING = 3511894251U;
        static const AkUniqueID PLAY_AI_SHOWER = 2950347541U;
        static const AkUniqueID PLAY_AI_TELEVISION = 861642119U;
        static const AkUniqueID PLAY_AMB_BATHROOM_LP = 2257634154U;
        static const AkUniqueID PLAY_AMB_KIDSBEDROOM_LP = 1402976205U;
        static const AkUniqueID PLAY_AMB_KITCHEN_LP = 4028544478U;
        static const AkUniqueID PLAY_AMB_LIVINGROOM_LP = 2639863138U;
        static const AkUniqueID PLAY_AMB_OUTSIDE_BALCONY_LP = 3232971858U;
        static const AkUniqueID PLAY_AMB_PARENTSBEDROOM_LP = 525979915U;
        static const AkUniqueID PLAY_AMBOUT_BATHROOM_NEIGHBORHOOD_LP = 1945404897U;
        static const AkUniqueID PLAY_AMBOUT_WALL_NEIGHBORHOOD_LP = 1682472951U;
        static const AkUniqueID PLAY_PLANE = 2689642136U;
        static const AkUniqueID PLAY_WASHER_CLOSE = 55201595U;
        static const AkUniqueID PLAY_WASHER_OPEN = 3531545949U;
        static const AkUniqueID PLAY_WINDOW_CLOSE = 4244658871U;
        static const AkUniqueID PLAY_WINDOW_OPEN = 4135004417U;
        static const AkUniqueID PLAYER_FOOTSTEPS = 1730208058U;
        static const AkUniqueID PLAYER_SHOWER_OFF = 3855133085U;
        static const AkUniqueID PLAYER_SHOWER_ON = 4187592433U;
        static const AkUniqueID PLAYER_SLAP = 2722341167U;
        static const AkUniqueID STOP_AI_CLEANBIN = 1064386371U;
        static const AkUniqueID STOP_AI_COOKING = 2408563091U;
        static const AkUniqueID STOP_AI_DISHES = 2349360669U;
        static const AkUniqueID STOP_AI_PEEING = 3353979151U;
        static const AkUniqueID STOP_AI_PEEINGFLOOR = 4242556375U;
        static const AkUniqueID STOP_AI_PLAYING = 2698630197U;
        static const AkUniqueID STOP_AI_SHOWER = 60512483U;
        static const AkUniqueID STOP_AI_TELEVISION = 4134788725U;
    } // namespace EVENTS

    namespace SWITCHES
    {
        namespace AI_ACTIVITY
        {
            static const AkUniqueID GROUP = 2723820741U;

            namespace SWITCH
            {
                static const AkUniqueID AI_CLEANBIN = 1845088756U;
                static const AkUniqueID AI_CLEANWINDOW = 1375722357U;
                static const AkUniqueID AI_COOKING = 2981324486U;
                static const AkUniqueID AI_DISHES = 3730865982U;
                static const AkUniqueID AI_FLUSH = 2553892380U;
                static const AkUniqueID AI_IDLE = 766486940U;
                static const AkUniqueID AI_PEEING = 3717382428U;
                static const AkUniqueID AI_PEEINGFLOOR = 719658054U;
                static const AkUniqueID AI_PLAYING = 3201920804U;
                static const AkUniqueID AI_SHOWER = 1157179748U;
                static const AkUniqueID AI_TELEVISION = 1990211566U;
            } // namespace SWITCH
        } // namespace AI_ACTIVITY

        namespace AI_MOVEMENTS
        {
            static const AkUniqueID GROUP = 1429789552U;

            namespace SWITCH
            {
                static const AkUniqueID AI_IDLE = 766486940U;
                static const AkUniqueID AI_STOP = 3628022334U;
                static const AkUniqueID AI_WALKING = 4179756143U;
            } // namespace SWITCH
        } // namespace AI_MOVEMENTS

        namespace AI_SHOWER
        {
            static const AkUniqueID GROUP = 1157179748U;

            namespace SWITCH
            {
                static const AkUniqueID AI_SHOWER_LP = 976917071U;
                static const AkUniqueID AI_SHOWER_OFF = 4046881850U;
                static const AkUniqueID AI_SHOWER_ON = 993694652U;
            } // namespace SWITCH
        } // namespace AI_SHOWER

        namespace PLAYER_SLAP
        {
            static const AkUniqueID GROUP = 2722341167U;

            namespace SWITCH
            {
                static const AkUniqueID CONCRETE = 841620460U;
                static const AkUniqueID KID = 1115957451U;
                static const AkUniqueID NOTHING = 4248742144U;
                static const AkUniqueID WOOD = 2058049674U;
            } // namespace SWITCH
        } // namespace PLAYER_SLAP

    } // namespace SWITCHES

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID AI = 1886858547U;
        static const AkUniqueID AMBIANCES = 1404066300U;
        static const AkUniqueID AMBIANCES_OUT = 1911419817U;
        static const AkUniqueID CHARACTER = 436743010U;
        static const AkUniqueID MENU = 2607556080U;
        static const AkUniqueID MUSICS = 1730564753U;
        static const AkUniqueID SFX = 393239870U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID APPARTMENT_REVERB = 2315521272U;
        static const AkUniqueID BATHROOM_REVERB = 2419867174U;
    } // namespace AUX_BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
