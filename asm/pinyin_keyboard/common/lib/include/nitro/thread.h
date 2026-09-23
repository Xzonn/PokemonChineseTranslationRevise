#ifndef NITRO_THREAD_H
#define NITRO_THREAD_H

#include <nds/ndstypes.h>

typedef struct {
    u64     div_numer;
    u64     div_denom;
    u64     sqrt;
    u16     div_mode;
    u16     sqrt_mode;
}
CPContext;

typedef struct
{
    u32     cpsr;
    u32     r[13];
    u32     sp;
    u32     lr;
    u32     pc_plus4;
    u32     sp_svc;
    CPContext cp_context;
}
OSContext;

typedef struct {
    OSContext context;
    u8 _[512 - sizeof(OSContext)];
}
OSThread;

void OS_CreateThread(OSThread *thread, void (*func) (void *), void *arg, void *stack, u32 stackSize, u32 prio);
void OS_WakeupThreadDirect(OSThread *thread);
void OS_ExitThread(void);
void OS_SleepThread(void *queue);

#endif
