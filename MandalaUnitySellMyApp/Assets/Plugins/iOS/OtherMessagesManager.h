#import <Foundation/Foundation.h>
#import "UnityAppController.h"



@interface OtherMessagesManager : NSObject <UIAlertViewDelegate>


+ (OtherMessagesManager*)sharedManager;

- (void)showAlert:(NSString*) message;

@end
