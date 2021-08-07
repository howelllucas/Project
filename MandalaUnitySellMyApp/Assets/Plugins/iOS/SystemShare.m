#import "SystemShare.h"
#import <SystemConfiguration/SystemConfiguration.h>

UIView* tempView;
UIActivityViewController* controller;

@implementation SystemShare

+(void)shareImage:(UIImage*)image atX:(CGFloat)x andY:(CGFloat)y
{
    NSArray* items = @[image];
    if (tempView != nil)
    {
        if (tempView)
        {
            [tempView removeFromSuperview];
        }
    }
    if (controller != nil)
    {
        if (controller)
        {
//            [controller removeFromParentViewController];
            [controller dismissViewControllerAnimated:YES completion:nil];
        }
    }
    
    /*UIActivityViewController**/ controller = [[UIActivityViewController alloc] initWithActivityItems:items applicationActivities:nil];
    CGRect location = CGRectMake(x, y, 1, 1);
    if ( UI_USER_INTERFACE_IDIOM()== UIUserInterfaceIdiomPhone)
    {
        controller.popoverPresentationController.sourceRect = location;
    }
  
    UIViewController* rootVC = UIApplication.sharedApplication.windows[0].rootViewController;
    /*UIView* */tempView = [[UIView alloc] initWithFrame:location];
    tempView.backgroundColor = [UIColor clearColor];
    [rootVC.view addSubview:tempView];
    
    controller.popoverPresentationController.sourceView = tempView;
    [controller setCompletionWithItemsHandler:^(NSString *activityType, BOOL completed, NSArray *returnedItems, NSError *activityError)
    {
        UnitySendMessage("WebelinxCMS", "CleanImage", "c");
        
        [tempView removeFromSuperview];
        
    }];
    [rootVC presentViewController:controller animated:YES completion:^{}];
    
    
}

@end
