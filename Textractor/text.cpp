#ifdef _WIN64
#define ARCH "x64"
#else
#define ARCH "x86"
#endif


// If you are updating a previous translation see https://github.com/Artikash/Textractor/issues/313
const wchar_t* CONSOLE = L"Console";
const wchar_t* CLIPBOARD = L"Clipboard";

const wchar_t* ALREADY_INJECTED = L"Textractor: already injected";
const wchar_t* NEED_32_BIT = L"Textractor: architecture mismatch: only Textractor x86 can inject this process";
const wchar_t* NEED_64_BIT = L"Textractor: architecture mismatch: only Textractor x64 can inject this process";
const wchar_t* INJECT_FAILED = L"Textractor: couldn't inject";
const wchar_t* LAUNCH_FAILED = L"Textractor: couldn't launch";
const wchar_t* INVALID_CODE = L"Textractor: invalid code";
const wchar_t* INVALID_CODEPAGE = L"Textractor: couldn't convert text (invalid codepage?)";

const char* PIPE_CONNECTED = u8"Textractor: pipe connected";
const char* INSERTING_HOOK = u8"Textractor: inserting hook: %s";
const char* REMOVING_HOOK = u8"Textractor: removing hook: %s";
const char* HOOK_FAILED = u8"Textractor: failed to insert hook";
const char* TOO_MANY_HOOKS = u8"Textractor: too many hooks: can't insert";

const char* HOOK_SEARCH_STARTING = u8"Textractor: starting hook search";
const char* HOOK_SEARCH_INITIALIZING = u8"Textractor: initializing hook search (%f%%)";
const char* HOOK_SEARCH_INITIALIZED = u8"Textractor: initialized hook search with %zd hooks";
const char* MAKE_GAME_PROCESS_TEXT = u8"Textractor: please click around in the game to force it to process text during the next %d seconds";
const char* HOOK_SEARCH_FINISHED = u8"Textractor: hook search finished, %d results found";
const char* OUT_OF_RECORDS_RETRY = u8"Textractor: out of search records, please retry if results are poor (default record count increased)";
const char* NOT_ENOUGH_TEXT = u8"Textractor: not enough text to search accurately";
const char* COULD_NOT_FIND = u8"Textractor: could not find text";

const char* FUNC_MISSING = u8"Textractor: function not present";
const char* MODULE_MISSING = u8"Textractor: module not present";
const char* GARBAGE_MEMORY = u8"Textractor: memory constantly changing, useless to read";
const char* SEND_ERROR = u8"Textractor: Send ERROR (likely an unstable/incorrect H-code)";
const char* READ_ERROR = u8"Textractor: Reader ERROR (likely an incorrect R-code)";
const char* HIJACK_ERROR = u8"Textractor: Hijack ERROR";

void Localize()
{
};

static auto _ = (Localize(), 0);
