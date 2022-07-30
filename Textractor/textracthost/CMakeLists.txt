set(textracthost_src
	dllmain.cpp
	pch.cpp
	host.cpp
	textthread.cpp
	hookcode.cpp
)
#target_precompile_headers(host REUSE_FROM pch)
add_compile_definitions(TEXTRACTHOST_EXPORTS)
add_library(textracthost MODULE ${textracthost_src})
