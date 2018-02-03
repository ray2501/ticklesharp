/*
 * TickleSharp sample - Scribble.cs
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
   public class TclScribble {
      public delegate int CmdProc (int argArrInst);
      static Tk tk;
      
      public static void Main (string[] args)
      {
         int iTclRet;
	 
         tk = new Tk ( );

         iTclRet = tk.Eval ("pack [canvas .c -bg blue] -fill both -expand 1");
	 tk.CreateCommand ("BeginDraw", new TclTkInterface.Tcl_CmdProc (BeginDraw));
	 tk.CreateCommand ("DrawPoint", new TclTkInterface.Tcl_CmdProc (DrawPoint));

         iTclRet = tk.Eval ("bind .c <1> [list BeginDraw %x %y]");
         iTclRet = tk.Eval ("bind .c <B1-Motion> {DrawPoint %x %y}");
         tk.SetWindowSize (".", 600, 600);
         tk.SetTitleBar (".", "SharpTk Scribble");
         tk.Run ( );
      }
      
      public static int BeginDraw (int ArgArrInst)
      {
         int iTclRet;
         String x, y;	 
         String Argv = tk.GetArgArray ("BeginDraw", ArgArrInst);
	 
         x = tk.GetParam (Argv, 1);
         y = tk.GetParam (Argv, 2);
	 
         iTclRet = tk.Eval ("set ::points [.c create line " +
                             x + " " + y + " " + 
                             x + " " + y + " -fill black]");
     	 
         tk.FreeArgArray (Argv);
         return 0;
      }
      
      public static int DrawPoint (int ArgArrInst)
      {
         int iTclRet;
         String x, y;	 
         String Argv = tk.GetArgArray ("DrawPoint", ArgArrInst);

         x = tk.GetParam (Argv, 1);
         y = tk.GetParam (Argv, 2);
         iTclRet = tk.Eval ("eval .c coords $::points [concat [.c coords " +
	                    "$::points] " + x + " " + y + "]");
         tk.FreeArgArray (Argv);
         return 0;
      }
   }   
}
