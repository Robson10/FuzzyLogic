﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuzzyLogic.Model;
using System.Windows;
//using FuzzyLogic.Model.ChartItems;
namespace FuzzyLogic
{
    class SystemWnioskowania
    {
        public double CenterOfGravityHeater = 0;
        public double CenterOfGravityConditioner = 0;
        public int temp = 0;
        public SystemWnioskowania()
        {
            //Rozmywanie();
        }
        //przyjmujemy wszystkie wartosci (pracujemy na referencjach wykresow ktore beda modyfikowane) potem wyciągamy indexy wrazie potrzeby(godzina,temp) i lecimy
        public void Rozmywanie( Heat Out1, Conditioner Out2, Temp InChart1, int value1,  WorkHour InChart2,int index2) 
        {
            int index1=0;
            for (int i = 0; i < InChart1.Count; i++)
            {
                if (InChart1[i].X==value1)
                {
                    index1 = i;
                }
            }
            temp = InChart1[index1].X;//dla Labela
            BlokWnioskowaniaOgrzewania(ref Out1,InChart1[index1],InChart2[index2]);
            BlokWnioskowaniaKlimatyzacji(ref Out2,InChart1[index1], InChart2[index2]);
        }
        //bloki romywania i wyostrzania metodą min max a nastepnie środekCiezkosci
        private void BlokWnioskowaniaOgrzewania(ref Heat Out, ChartItem5Value in1, ChartItem1Value in2)
        {
            double min, low, mid, mor, max, join;
            for (int i = 0; i < Out.Count; i++)
            {
                if (in1.Min + in1.Low + in1.Mid < 1 && in1.Mor + in1.Max > 0)
                    min = Min(in2.Y, 1, Out[i].Min);
                else
                    min = Min(in2.Y, in1.Mid, Out[i].Min);
                mid = Min(in2.Y, in1.Low, Out[i].Mid);
                if (in1.Min + in1.Low + in1.Mid < 1 && in1.Mor + in1.Max == 0)
                    max = Min(in2.Y, 1, Out[i].Max);
                else
                    max = Min(in2.Y, in1.Min, Out[i].Max);
                mor = 0;
                low = 0;
                join = Max(min, low, mid, mor, max);

                Out[i].ZbiorWnioskowania = join;
                Out[i].Text = Out.AddTextJoin(Out[i].Text, join);
            }
            BlokWyostrzania(ref Out);
        }
        private void BlokWyostrzania(ref Heat Out)
        {
            double up = 0;
            double above = 0;
            for (int i = 1; i < Out.Count; i++)
            {
                up += Out[i].X * Out[i].ZbiorWnioskowania;
                above += Out[i].ZbiorWnioskowania;
            }
               CenterOfGravityHeater=(above == 0)?0: Math.Round(up / above);
        }
        private void BlokWnioskowaniaKlimatyzacji(ref Conditioner Out, ChartItem5Value in1, ChartItem1Value in2)
        {
            double min, low, mid, mor, max, join;
            for (int i = 0; i < Out.Count; i++)
            {
                if (in1.Max + in1.Mor + in1.Mid < 1 && in1.Min + in1.Low > 0)
                    min = Min(in2.Y, 1, Out[i].Min);
                else
                    min = Min(in2.Y, in1.Mid, Out[i].Min);

                mid = Min(in2.Y, in1.Mor, Out[i].Mid);
                if (in1.Max + in1.Mor + in1.Mid < 1 && in1.Min + in1.Low == 0)
                    max = Min(in2.Y, 1, Out[i].Max);
                else
                    max = Min(in2.Y, in1.Max, Out[i].Max);

                low = 0;
                mor = 0;

                join = Max(min, low, mid, mor, max);

                Out[i].ZbiorWnioskowania = join;
                Out[i].Text = Out.AddTextJoin(Out[i].Text, join);
            }
            BlokWyostrzania(ref Out);
        }
        private void BlokWyostrzania(ref Conditioner Out)
        {
            double up = 0;
            double above = 0;
            for (int i = 1; i < Out.Count; i++)
            {
                up += Out[i].X * Out[i].ZbiorWnioskowania;
                above += Out[i].ZbiorWnioskowania;
            }
            CenterOfGravityConditioner = (above == 0) ? 0 : Math.Round(up / above);
        }
        
        private double Min(params double[] Values)
        {
            return Values.Min();
        }
        private double Max(params double[] Values)
        {
            return Values.Max();
        }
        
    }
}
