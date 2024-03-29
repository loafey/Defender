﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defender {
    class MathExtra {
        public MathExtra() { }
        public static float Lerp(float n, float goal, float multiplier) {
            return n * (1 - multiplier) + goal * multiplier;
        }

        public static float GridLocation(int n, int size) {
            return n * size;
        }

        public static double GetDistance(double x1, double y1, double x2, double y2) {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public static float GetDistanceAxisAbs(float n1, float n2) {
            return Math.Abs(n1 - n2);
        }

        public static float GetDistanceAxis(float n1, float n2) {
            return n1 - n2;
        }

        public static bool PlayerBlockAABB(Player player, Block block) {
            if(player.x < block.x + block.width &&
                player.x + player.width > block.x &&
                player.y < block.y + block.height &&
                player.y  + player.height > block.y) {
                return true;
            } else {
                return false;
            }
        }

        public static bool AABB(float x1, float y1, float width1, float height1, float x2, float y2, float width2, float height2) {
            if (x1 < x2 + width2 &&
                x1 + width1 > x2 &&
                y1 < y2 + height2 &&
                y1 + height1 > y2) {
                return true;
            } else {
                return false;
            }
        }

        //Currently not implemted due to buggy nature, replaced by GetDistance
        /*public static bool checkCollisionPlayerBlock(Player player, Block block) {
            if(Math.Abs(player.x - block.x ) < player.width + block.width ) {
                Console.WriteLine("Colliding on X. Player X: {0}. Block X: {1}", player.x, block.x);
                //Console.WriteLine("Colliding on X. Player width: {0}. Block width: {1}", player.width, block.width);
                Console.WriteLine(Math.Abs(player.x - block.x) + " < " + (player.width + block.width));
                return true;
                if (Math.Abs(player.y  - block.y ) < player.y + block.x ) {
                    //Console.WriteLine("Colliding on Y");
                    return true;
                }
            }
            return false;
        } */
    }
}
