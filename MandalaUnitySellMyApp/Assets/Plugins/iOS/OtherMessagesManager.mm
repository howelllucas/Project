#import "OtherMessagesManager.h"
#import "SaveToGallery.h"
#import "SystemShare.h"
//void UnitySendMessage( const char * className, const char * methodName, const char * param );

void UnityPause( bool pause );




@implementation OtherMessagesManager

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

+ (OtherMessagesManager*)sharedManager // vraca singleton objekat
{
	static OtherMessagesManager *sharedSingleton;
	
	if( !sharedSingleton )
		sharedSingleton = [[OtherMessagesManager alloc] init];
	
	return sharedSingleton;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)showAlert:(NSString*) message
{
    NSLog(@"Other Messages Manager --- Poruka od UNitija: %@", message);
    NSString* messageCommand = @"";
    NSArray* listMessage = [message componentsSeparatedByString:@"###"];
    
    if ([listMessage count] > 1)
   {
        messageCommand = [listMessage objectAtIndex:0];
       
        if ([messageCommand isEqualToString:@"SaveToGallery"])
        {
            [[SaveToGallery sharedSaveToGallery] saveToGalleryWithPath:[listMessage objectAtIndex:1]];
        }
       else if([messageCommand isEqualToString:@"ShareImage"])
       {
            NSString* path = @"";
           path=[listMessage objectAtIndex:3];
           int xPos=[[listMessage objectAtIndex:1] intValue];
           int yPos=[[listMessage objectAtIndex:2] intValue];
           
           CGFloat x=((CGFloat)xPos)/((CGFloat)[[UIScreen mainScreen] scale]);///scale;
           CGFloat y=((CGFloat)yPos)/((CGFloat)[[UIScreen mainScreen] scale]);
           CGRect screenRect = [[UIScreen mainScreen] bounds];
//           CGFloat screenWidth = screenRect.size.width;
           CGFloat screenHeight = screenRect.size.height;
           y=screenHeight-y;
//           x=screenWidth-x;
           
           NSData* tempData = [NSData dataWithContentsOfFile:path];
//
           if (tempData)
           {
            UIImage* tempImage = [UIImage imageWithData:tempData];
               if (tempImage)
                {
                     NSLog(@"System share started");
                    NSLog(@"POSITION: %f  %f",x,y);
                    
                    
                    [SystemShare shareImage :tempImage atX:x andY:y];
                }
                else
                {
                    NSLog(@"Error code. Image Nil");
                }

           }
           else
           {
               NSLog(@"Error code. Data NIL");
           }
           
       }
    }//*/
    if([message isEqualToString:@"CheckForFBApp"])
    {
        if ([[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"fb://"]])
        {
            UnitySendMessage("WebelinxCMS","CheckForFBAppResult","true");
        }
        else
        {
            UnitySendMessage("WebelinxCMS","CheckForFBAppResult","false");
        }
    }
}


@end
