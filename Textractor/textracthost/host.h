#pragma once

#ifdef TEXTRACTHOST_EXPORTS
#define TEXTRACTHOST_API __declspec(dllexport)
#else
#define TEXTRACTHOST_API __declspec(dllimport)
#endif

#include "common.h"
#include "textthread.h"

using ProcessEventHandler = void(*)(DWORD);
using ThreadEventCallback = void(*)(struct OutputInfo);
using ThreadEventHandler = std::function<void(TextThread&)>;
using HookEventHandler = std::function<void(HookParam, std::wstring text)>;

extern "C" TEXTRACTHOST_API void __stdcall Start(ProcessEventHandler Connect, ProcessEventHandler Disconnect, ThreadEventCallback Create, ThreadEventCallback Destroy, TextThread::OutputCallback Output);
extern "C" TEXTRACTHOST_API void __stdcall InjectProcess(DWORD processId);
extern "C" TEXTRACTHOST_API void __stdcall DetachProcess(DWORD processId);
extern "C" TEXTRACTHOST_API void __stdcall InsertHook(DWORD processId, HookParam hp);
extern "C" TEXTRACTHOST_API void __stdcall RemoveHook(DWORD processId, uint64_t address);
void FindHooks(DWORD processId, SearchParam sp, HookEventHandler HookFound = {});

TextThread* GetThread(int64_t handle);
TextThread& GetThread(ThreadParam tp);

void AddConsoleOutput(std::wstring text);

inline int defaultCodepage = SHIFT_JIS;

constexpr ThreadParam console{ 0, -1LL, -1LL, -1LL }, clipboard{ 0, 0, -1LL, -1LL };
