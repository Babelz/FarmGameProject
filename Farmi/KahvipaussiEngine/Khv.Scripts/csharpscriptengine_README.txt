﻿Configurationfilen rakenne.

<ROOT>
	<!-- Assemblyt jotka viedään scriptiin sen kääntövaiheessa -->
	<ScriptDepencies>
		<!-- Lisää scripti moottoria käyttävän asssemblyn viitteen -->
		<ScriptDepency>this</ScriptDepency>
		<ScriptDepency>mscorlib.dll</ScriptDepency>
	</ScriptDepencies>

	<!-- Polut joista etsitään scriptejä -->
	<ScriptPaths>
		<ScriptPath>C:\Scriptit</ScriptPath>
	</ScriptPaths>

	<!-- Tiedosto päätteet joita scriptit käyttävät -->
	<ScriptFileExtensions>
		<ScriptFileExtension>.cs</ScriptFileExtension>
		<ScriptFileExtension>.txt</ScriptFileExtension>
	</ScriptFileExtensions>
</ROOT>