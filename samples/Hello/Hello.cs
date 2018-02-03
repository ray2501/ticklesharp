/*
 * TickleSharp sample - Hello.cs
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
   public class Hello {
      public delegate int CmdProc (int argArrInst);
      static Tk tk;
   
      public static void Main (string[] args)
      {
         int iTclRet = 2;

         tk = new Tk ( );

         iTclRet = tk.Eval ("wm title . {Hello World!}");
         Console.WriteLine (iTclRet);
         iTclRet = tk.Eval ("button .b -text {Hello World!} -command {testcmd} -padx 20");
         Console.WriteLine (iTclRet);
         iTclRet = tk.Eval ("button .e -text {Exit!} -command exit");
         Console.WriteLine (iTclRet);
         iTclRet = tk.Eval ("pack .b");
         Console.WriteLine (iTclRet);
         iTclRet = tk.Eval ("pack .e");
         Console.WriteLine (iTclRet);
         Console.WriteLine (tk.Result);
	 
         tk.CreateCommand ("testcmd", new TclTkInterface.Tcl_CmdProc (MyCmdProc));
         tk.Run ( );
      }
   
      public static int MyCmdProc (int ArgArrInst)
      {
         int iTclRet, iNumofParams;
         String Argv = tk.GetArgArray ("testcmd", ArgArrInst);
	 
         iTclRet = tk.Eval (".b configure -text {Goodby World!} -padx 20");
         Console.WriteLine (iTclRet);                 	 
         iNumofParams = int.Parse (tk.GetParam (Argv, 0));
         Console.WriteLine ("Number of args sent to callback: {0}", iNumofParams);
         Console.WriteLine ("This is a test.");

         tk.FreeArgArray (Argv);
         return 0;
      }
   }
}
