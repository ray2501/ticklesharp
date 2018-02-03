/*
 * TickleSharp sample - tcltest.cs
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
   public class test {
      public delegate int CmdProc (int argArrInst);
      static Tcl tcl;
   
      public static void Main (string[] args)
      {
         int iTclRet = 2, iTest = 5;
         String strtest = new String (' ', 255);

         tcl = new Tcl ( );

         iTclRet = tcl.Eval ("puts {Hello World!}");
         Console.WriteLine (iTclRet);                 	 
         iTclRet = tcl.Eval ("puts {How are you?}");
         Console.WriteLine (iTclRet);
         Console.WriteLine (tcl.Result);

         iTclRet = tcl.Eval ("set xx {This is it!}");
         Console.WriteLine (iTclRet);
         String str = tcl.GetVar ("xx");
         Console.WriteLine (str);

         tcl.SetVar ("xx", "This was it!");
         String str2 = tcl.GetVar ("xx");
         Console.WriteLine (str2);
	 
	 tcl.CreateCommand ("CSharpProc", new TclTkInterface.Tcl_CmdProc (localCSharpProc));
         iTclRet = tcl.Eval ("CSharpProc {Neo} {Smith}");

         iTclRet = tcl.Eval ("set ccc 69");
         Console.WriteLine (iTclRet);
         iTclRet = tcl.Eval ("puts $ccc");
         Console.WriteLine (iTclRet);
         Console.WriteLine (iTest);
      }
      
      public static int localCSharpProc (int ArgArrInst)
      {
         int iNumofParams;
         String param1;
         String Argv = tcl.GetArgArray ("CSharpProc", ArgArrInst);
  
         iNumofParams = int.Parse (tcl.GetParam (Argv, 0));
         Console.WriteLine ("Number of args sent to callback: {0}", iNumofParams);
         param1 = tcl.GetParam (Argv, 1);
         Console.WriteLine ("{0} the Matrix has you....", param1);
         Console.WriteLine ("{0} is a program.", tcl.GetParam (Argv, 2));

         tcl.FreeArgArray (Argv);
         return 0;	 
      }      
   }
}
