#Setting Up My Personal Website

This is where I am going to write about setting up my website on azure and github and how the asp.net mvc project is structure for creating this post.

##Blog

I have become a big fan of writing using markdown and I would like to write my blog posts with it. I also want to be able to write offline. I would like to have my blog posts locally in a folder and I would like to be able edit the files using my favorite markdown editor (currently this is MarkdownPad). I want to specify metadata for each post such as:

* related posts
* tags
* post date
* title
* author

I want to use git to manage the content. History, reviews, collaboration. When I push to the master, that is when I want to make my post public on the website. I am toying with the idea of having a branch per post. Not sure if that is necessary at this point.

I needed to decide if it made sense to separate the markdown files (my blog posts) from the regular html code in my Views folder. Every time I push changes to master the entire website is deployed. I had to rethink this and decide if I wanted to have a different history that shows changes to the website from the history of the blog content. Instead of using markdown, I could just write my posts in html. If I was to do this, I would probably just create a razor view in the Views folder. Typically I would want to separate my content from my presentation but when I write a blog post I am creating content but I want to control how it is presented at the same time. So the need to separate out the presentation does not seem to apply here. For now I am just going to keep my markdown files in the App\_Data folder and have one history for everything. Eventually I will probably want to have the history for my blog content separate from the history of my blog system. I also will probably want to move the content out of the App_Data folder and move it somewhere on storage or something. Maybe into its own repo with a simple Web Api on top of it.

##Relative Paths
Content.Url is the typical way to ensure that all the paths referenced will point to the intended path.
