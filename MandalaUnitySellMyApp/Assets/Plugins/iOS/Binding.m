#import "OtherMessagesManager.h"

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

// Converts C style string to NSString as long as it isnt empty
#define GetStringParamOrNil( _x_ ) ( _x_ != NULL && strlen( _x_ ) ) ? [NSString stringWithUTF8String:_x_] : nil

void _sendMessageOther( const char * appSignature ) // primamo poruku od Unity i prosledjujemo  OtherMessagesManager
{
    [[OtherMessagesManager sharedManager] showAlert:[NSString stringWithUTF8String:appSignature]];
}



