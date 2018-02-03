/*
 * TickleSharp sample - TclSharpCalc.cs
 *
 * Copyright (C) 2003  Scott Beasley.
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using TickleSharp;

namespace TickleSharpTest {
   public class TclCalc {
      public delegate int CmdProc (int argArrInst);
      static Tk tk;
      
      public static void Main (string[] args) 
      {
         int iTclRet;
	 
         tk = new Tk ( );

         // Build the Calculator display.
         iTclRet = tk.Eval ("entry .display -textvariable calcdisplay -justify right -takefocus 0\n" +
                            "grid .display -sticky new -row 0 -columnspan 4\n" +
	                    "set ind 4\n" +
                            "foreach button_info [list {9 nine} {8 eight} {7 seven} {* mult} {6 six} {5 five} {4 four} {/ div} {3 three} {2 two} {1 one} {= equ} {+ plus} {0 zero} {- minus} {. dec} {c clear} {MC mc}] { " +
                            "   set b [lindex $button_info 0]\n" +
                            "   set name [lindex $button_info 1]\n" + 
			    "   button .$name -text $b -command [list CalcButton $b] -bd 1 -padx 5\n" + 
			    "	grid .$name -row [expr $ind/4] -column [expr $ind % 4]\n" +
			    "	incr ind " +
			    "}");
	 tk.CreateCommand ("CalcButton", new TclTkInterface.Tcl_CmdProc (CalcButton));

         tk.SetWindowSize (".", 150, 175);
         tk.SetTitleBar (".", "TickleSharp Calc");
         tk.Run ( );
      }
      
      public static int CalcButton (int ArgArrInst)
      {
         String CalcDisplay, button;
         String Argv = tk.GetArgArray ("CalcButton", ArgArrInst);
         
         button = tk.GetParam (Argv, 1);
         CalcDisplay = tk.GetVar ("calcldispaly");
	 
         switch (button) {
            case "+":
              	    
              break;

            case "-":
              break;

            case "*":	 
              break;

            case "/":	 
              break;

            case "=":	 
              break;

            case "c":
              break;

            case "MC":
              break;

            default:
              tk.Eval (".display insert end " + button);
              break;	      
         }
	 
//         Console.WriteLine (CalcDisplay);
//         tk.SetVar ("calcldispaly", CalcDisplay);
//         holder = Convert.ToDecimal (CalcDisplay);

         tk.FreeArgArray (Argv);
         return 0;
      }
   }
}
