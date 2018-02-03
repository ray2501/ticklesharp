/*
 * TickleSharp - Classes that offer Tcl/Tk binding to .NET
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
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

namespace TickleSharp {
   // Interface Class to only be used by the Tcl and
   // Tk classes internally!
   public class TclTkInterface {
      #if UNIX_SYSTEM
         const String TCLDLL = "libtcl8.6.so";
         const String TKDLL = "libtk8.6.so";
         const String TCLWRAPPERDLL = "libtclwrapper.so";
      #endif
      #if MSWINDOWS_SYSTEM
         const String TCLDLL = "tcl86.dll";
         const String TKDLL = "tk86.dll";
         const String TCLWRAPPERDLL = "tclwrapper.dll";
      #endif

      // Things the CreateCommand & others need.
      public delegate int Tcl_CmdProc (int ArgArr);
      public delegate int Tcl_PackageInitProc (IntPtr ip);

      // See tcl.h and tclDecls.h or the man pages for more details.
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public void Tcl_FindExecutable (String argv0);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public IntPtr Tcl_CreateInterp ( );
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public void Tcl_StaticPackage (IntPtr ip, String pkgName, Tcl_PackageInitProc initProc, Tcl_PackageInitProc safeInitProc);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public int Tcl_DeleteInterp (IntPtr ip);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public int Tcl_Init (IntPtr ip);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public int Tcl_Eval (IntPtr ip, String strng);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public IntPtr Tcl_GetVar (IntPtr ip, String varName, int flags);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public IntPtr Tcl_GetVar2 (IntPtr ip, String varName, String elmName, int flags);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public IntPtr Tcl_SetVar (IntPtr ip, String varName, String value, int flags);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public int Tcl_UnsetVar (IntPtr ip, String varName, int flags);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public IntPtr Tcl_GetObjResult (IntPtr ip);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public int Tcl_SetObjResult (IntPtr ip, IntPtr objPtr);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public IntPtr Tcl_GetStringFromObj (IntPtr tclObj, IntPtr len);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public IntPtr Tcl_GetString (IntPtr tclObj);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public IntPtr Tcl_GetStringResult (IntPtr ip);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public IntPtr Tcl_NewStringObj (String bytes,int length);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public IntPtr Tcl_NewIntObj (int intValue);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public IntPtr Tcl_NewDoubleObj (double doubleValue);
      [DllImport(TclTkInterface.TCLDLL, CallingConvention=CallingConvention.Cdecl)] static extern public IntPtr Tcl_NewBooleanObj (bool booleanValue);
      
      // See tk.h and tkDecls.h or the man pages for more details.
      [DllImport(TclTkInterface.TKDLL, CallingConvention=CallingConvention.Cdecl)] static extern public int Tk_Init (IntPtr interp);
      [DllImport(TclTkInterface.TKDLL, CallingConvention=CallingConvention.Cdecl)] static extern public void Tk_MainLoop ( );

      // Wrapper for Tcl_CreateObjCommand.
      [DllImport(TclTkInterface.TCLWRAPPERDLL)] static extern public int CreateCommand (IntPtr ip, String cmdName, Tcl_CmdProc proc);
   }

   public abstract class TclTk {
      // Things the CreateCommand & others need.
      public delegate int Tcl_CmdProc (int ArgArr);
      private Hashtable callbacksprocs = new Hashtable ( );
      
      // Common Tcl return codes.   
      public enum TclReturn : int {
         TCL_OK = 0,
         TCL_ERROR = 1
      }

      // Flags used for variable functions.
      public enum TclVarFlags : int {
         TCL_GLOBAL_ONLY = 1,
         TCL_NAMESPACE_ONLY = 2,
         TCL_APPEND_VALUE = 4,
         TCL_LIST_ELEMENT = 8
      }

      // Tcl variable type for Tcl_LinkVar.
      public enum TclVarTypes : int {
         TCL_LINK_INT = 1,
         TCL_LINK_DOUBLE = 2,
         TCL_LINK_BOOLEAN = 3,
         TCL_LINK_STRING = 4,
         TCL_LINK_READ_ONLY = 128
      }

      protected static IntPtr ip;

      public int Eval (String strCmds)
      {
         return TclTkInterface.Tcl_Eval (ip, strCmds);
      }

      public String GetVar (String strVarName)
      {
         unsafe {
            return Marshal.PtrToStringAnsi (TclTkInterface.Tcl_GetVar (ip, strVarName, 0));
         }
      }      

      public String GetVar (String strVarName, int flags)
      {
         unsafe {
            return Marshal.PtrToStringAnsi (TclTkInterface.Tcl_GetVar (ip, strVarName, flags));
         }
      }      

      public String GetVar (String strVarName, String strElmName)
      {
         unsafe {
            return Marshal.PtrToStringAnsi (TclTkInterface.Tcl_GetVar2 (ip, strVarName, strElmName, 0));
         }
      }      

      public String GetVar (String strVarName, String strElmName, int flags)
      {
         unsafe {
            return Marshal.PtrToStringAnsi (TclTkInterface.Tcl_GetVar2 (ip, strVarName, strElmName, flags));
         }
      }      

      public String SetVar (String strVarName, String strVal)
      {
         return Marshal.PtrToStringAnsi (TclTkInterface.Tcl_SetVar (ip, strVarName, strVal, 0));
      }      

      public String SetVar (String strVarName, String strVal, int flags)
      {
         return Marshal.PtrToStringAnsi (TclTkInterface.Tcl_SetVar (ip, strVarName, strVal, flags));
      }      

      public String GetArgArray (String strProcName, int ElmName)
      {
         return String.Format ("{0}{1,-00000:d}",
                                strProcName, ElmName);
      }      

      public void FreeArgArray (String strArrayName)
      {      
         TclTkInterface.Tcl_UnsetVar (ip, strArrayName, 0);
      }
      
      public int CreateCommand (String cmdName, TclTkInterface.Tcl_CmdProc proc)
      {
         // Force a reference to kept of the callback so it will not be GC'ed.      
         callbacksprocs.Add (cmdName, proc);
	 
         return TclTkInterface.CreateCommand (ip, cmdName, proc);
      }

      public String GetParam (String paramArray, int iNdx)
      {
         unsafe {
            return Marshal.PtrToStringAnsi (TclTkInterface.Tcl_GetVar2 (ip, paramArray, iNdx.ToString ( ), 0));
         }
      }

      public IntPtr CreateTclObject (Object dotnetobj)
      {
         // 
         // We take advantage here that all .NET objects are based off the 
         // Object class.
         //
	 
	 // May want to Marshal each one of these out for better conversion.

         if (dotnetobj is String) {
            return TclTkInterface.Tcl_NewStringObj ((String)dotnetobj,
                                                   ((String)dotnetobj).Length);
         } else if (dotnetobj is Int32) {
            return TclTkInterface.Tcl_NewIntObj ((int)dotnetobj);
         } else if (dotnetobj is Double) {
            return TclTkInterface.Tcl_NewDoubleObj ((double)dotnetobj);
         } else if (dotnetobj is Boolean) {
            return TclTkInterface.Tcl_NewBooleanObj ((bool)dotnetobj);
         }

         // Not sure if this is the best thing to do, but for now.....
         return IntPtr.Zero;	 
      }

      public int SetTclResult (Object dotnetobj)
      {
         return TclTkInterface.Tcl_SetObjResult (ip, CreateTclObject (dotnetobj));
      }
      
      public String Result
      {
         get {
            return Marshal.PtrToStringAnsi (TclTkInterface.Tcl_GetStringResult (ip));
         }	    
      }
   }
   
   public class Tcl : TclTk {
      public Tcl ( )
      {
         int iRet;
               
         TclTkInterface.Tcl_FindExecutable ("");
	    
         ip = TclTkInterface.Tcl_CreateInterp ( );
         if (ip == IntPtr.Zero) {
            throw new SystemException ("Failed on Tcl_CreateInterp call.");	    
         } else {
            // Force load of Tcl init.tcl scripts.
            iRet = TclTkInterface.Tcl_Init (ip);
            if (iRet != (int)TclReturn.TCL_OK) {
               throw new SystemException ("Failed on Tcl_Init call.");	    
            }	       
         }	    
      }
   }
   
   public class Tk : TclTk {
      public Tk ( )
      {
         int iRet;

         TclTkInterface.Tcl_FindExecutable ("");
	    
         ip = TclTkInterface.Tcl_CreateInterp ( );
         if (ip == IntPtr.Zero) {
            throw new SystemException ("Failed on Tcl_CreateInterp call.");	    
         } else {
            // Force load of Tcl & Tk init.tcl (tk.tcl) scripts.
            iRet = TclTkInterface.Tcl_Init (ip);
            if (iRet != (int)TclReturn.TCL_OK) {
               throw new SystemException ("Failed on Tcl_Init call.");	    
            }
	       
            iRet = TclTkInterface.Tk_Init (ip);
            if (iRet != (int)TclReturn.TCL_OK) {
               throw new SystemException ("Failed on Tk_Init call.");
            }
         }	       
      }	 
      
      public int Run ( )
      {
         // Call Tk_MainLoop to process the GUI and
         // drive the rest of the program.
         TclTkInterface.Tk_MainLoop ( );
         TclTkInterface.Tcl_DeleteInterp (ip);

         return (int)TclReturn.TCL_OK;
      }
      
      public int SetTitleBar (String window, String title)
      {
         return TclTkInterface.Tcl_Eval (ip, "wm title " + window + " {" + title + "}");
      }
      
      public int SetWindowGeometry (String window, int wid, int hgt, int xpos, int ypos)
      {
         return  TclTkInterface.Tcl_Eval (ip, "wm geometry " + window + " " +
                                          wid.ToString ( ) + "x" + hgt.ToString( ) + "+" + 
					  xpos.ToString ( ) + "+" + ypos.ToString ( ));
      }      

      public int SetWindowSize (String window, int wid, int hgt)
      {
         return  TclTkInterface.Tcl_Eval (ip, "wm geometry " + window + " " +
                                          wid.ToString ( ) + "x" + hgt.ToString( ));
      }      

      public int SetWindowPos (String window, int xpos, int ypos)
      {
         return  TclTkInterface.Tcl_Eval (ip, "wm geometry " + window + "+" +
                                          xpos.ToString ( ) + "+" + ypos.ToString( ));
      }      

      public int MaximizeWindow (String window)
      {
         return  TclTkInterface.Tcl_Eval (ip, "wm deiconify " + window);
      }

      public int MinimizeWindow (String window)
      {
         return  TclTkInterface.Tcl_Eval (ip, "wm iconify " + window);
      }
      
      public int DestroyWindow (String window)
      {
         return  TclTkInterface.Tcl_Eval (ip, "destroy " + window);
      }
      
      public int SetFocus (String window)
      {
         return  TclTkInterface.Tcl_Eval (ip, "focus " + window);
      }      
   }
}   
