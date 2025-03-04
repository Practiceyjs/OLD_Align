using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace T_Align
{
    static public class DIsplay_Info
    {
        //가로 Display count
        //세로 Type
        /////디스플레이 1개 / 유닛 1개 -> 1,0    로케이션0   OK
        /////디스플레이 2개 / 유닛 1개 -> 2,0    로케이션0   OK  ???
        /////디스플레이 2개 / 유닛 2개 -> 1,1    로케이션1   OK
        /////디스플레이 4개 / 유닛 1개 -> 4,0    로케이션0   OK
        ///
        /////디스플레이 4개 / 유닛 2개 -> 2,0    로케이션0   OK
        /////디스플레이 4개 / 유닛 4개 -> 1,2    로케이션0   OK
        /////디스플레이 8개 / 유닛 2개 -> 4,1    로케이션1   OK
        /////디스플레이 8개 / 유닛 4개 -> 2,1    로케이션1   OK   
        /////디스플레이 16개 / 유닛 4개 -> 4,2   로케이션0   OK
        //.....공백 버려

        static public Size[,] Size = {  { new Size(1, 1), new Size(1, 1), new Size(1, 1) },
                                        { new Size(1154, 916), new Size(916, 719), new Size(566, 434) },
                                        { new Size(574, 433), new Size(455, 334), new Size(280, 192) },
                                        { new Size(566, 455), new Size(447, 356), new Size(272, 214) },
                                        { new Size(574, 455), new Size(455, 356), new Size(280, 214) } };

        static public Size[,] Panel_Size = {    { new Size(50, 50), new Size(1, 1), new Size(1, 1) } ,
                                                { new Size(1170, 958), new Size(932, 761), new Size(582, 476) },
                                                { new Size(1170, 475), new Size(932, 376), new Size(582, 234) },
                                                { new Size(582, 958), new Size(463, 761), new Size(288, 476) },
                                                { new Size(1170, 958), new Size(932, 761), new Size(582, 476) }};

        static public Size[,] Log_Size = {  { new Size(694, 957), new Size(694, 476),new Size(1, 1), new Size(694, 235) },
                                            { new Size(1870, 193), new Size(932, 193),new Size(1, 1), new Size(463, 193) }};

        static public Point[,] Log_Location = { { new Point(1182, 4), new Point(1182, 245), new Point(1182, 485), new Point(1182, 726) },
                                                { new Point(6, 768), new Point(475, 768), new Point(944, 768), new Point(1413, 768) } };

        /////디스플레이 1개 / 유닛 1개 -> 1,0    로케이션0   OK
        /////디스플레이 2개 / 유닛 1개 -> 2,0    로케이션0   OK
        /////디스플레이 2개 / 유닛 2개 -> 1,1    로케이션1   OK
        /////디스플레이 4개 / 유닛 1개 -> 4,0    로케이션0   OK
        /////디스플레이 4개 / 유닛 2개 -> 2,1    로케이션0   OK
        /////디스플레이 4개 / 유닛 4개 -> 1,2    로케이션0   OK
        /////디스플레이 8개 / 유닛 2개 -> 4,1    로케이션1   OK
        /////디스플레이 8개 / 유닛 4개 -> 2,1    로케이션1   OK   
        /////디스플레이 16개 / 유닛 4개 -> 4,2   로케이션0   OK

        static public Point[,] Display_Location2 = { { new Point(6, 4), new Point(594, 4), new Point(6, 485), new Point(594, 485) },
                                                    { new Point(6, 4), new Point(944, 4), new Point(6, 386), new Point(944, 386) }};
        static public Point[,,] Display_Location = {
            {
            { new Point(0, 0),new Point(0, 0),new Point(0, 0),new Point(0, 0) },
            { new Point(0, 0),new Point(0, 0),new Point(0, 0),new Point(0, 0) },
            { new Point(0, 0),new Point(0, 0),new Point(0, 0),new Point(0, 0) }},

            {
            { new Point(6, 4), new Point(594, 4), new Point(6, 485), new Point(594, 485)},
            { new Point(6, 4), new Point(594, 4), new Point(6, 485), new Point(594, 485) },
            { new Point(6, 4), new Point(944, 4), new Point(6, 386), new Point(944, 386) }},

            {
            { new Point(6, 4), new Point(594, 4), new Point(6, 485), new Point(594, 485)},
            { new Point(6, 4), new Point(594, 4), new Point(6, 485), new Point(594, 485)},
            { new Point(6, 4), new Point(944, 4), new Point(6, 386), new Point(944, 386) }},

            {
            { new Point(6, 4),new Point(0, 0),new Point(0, 0),new Point(0, 0) },
            { new Point(6, 4),new Point(0, 0),new Point(0, 0),new Point(0, 0) },
            { new Point(6, 4),new Point(0, 0),new Point(0, 0),new Point(0, 0) } },

            {
            { new Point(6, 4), new Point(594, 4), new Point(6, 485), new Point(594, 485) },
            { new Point(6, 4), new Point(594, 4), new Point(6, 485), new Point(594, 485) },
            { new Point(6, 4), new Point(944, 4), new Point(6, 386), new Point(944, 386) }}};
    }
}
