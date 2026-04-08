-- Fix Bangla Content for Privacy Policy and Terms & Conditions Pages
-- This script uses NVARCHAR to properly store Unicode (Bangla) characters
USE [WBS_NGO];
GO

-- Delete existing pages if they have encoding issues
DELETE FROM Pages WHERE Slug IN ('privacy-policy', 'terms-conditions');
GO

-- Insert Privacy Policy Page with proper Unicode support
INSERT INTO Pages (Title, TitleBn, Slug, Content, ContentBn, MetaTitle, MetaDescription, IsActive, ShowInFooter, CreatedAt, CreatedBy)
VALUES (
    N'Privacy Policy',
    N'গোপনীয়তা নীতি',
    'privacy-policy',
    N'<h2>Privacy Policy</h2>
<p><strong>Effective Date:</strong> January 2025</p>

<h3>1. Introduction</h3>
<p>Working Bangladesh Society (WBS) is committed to protecting your privacy. This Privacy Policy explains how we collect, use, disclose, and safeguard your information when you visit our website or interact with our services.</p>

<h3>2. Information We Collect</h3>
<h4>2.1 Personal Information</h4>
<p>We may collect personally identifiable information such as:</p>
<ul>
    <li>Name</li>
    <li>Email address</li>
    <li>Phone number</li>
    <li>Mailing address</li>
    <li>Payment information (when making donations)</li>
</ul>

<h4>2.2 Usage Data</h4>
<p>We automatically collect certain information when you visit our website, including:</p>
<ul>
    <li>IP address</li>
    <li>Browser type and version</li>
    <li>Pages visited</li>
    <li>Time and date of visit</li>
    <li>Time spent on pages</li>
</ul>

<h3>3. How We Use Your Information</h3>
<p>We use the information we collect to:</p>
<ul>
    <li>Process donations and provide receipts</li>
    <li>Send newsletters and updates about our programs</li>
    <li>Respond to inquiries and provide support</li>
    <li>Improve our website and services</li>
    <li>Comply with legal obligations</li>
</ul>

<h3>4. Information Sharing</h3>
<p>We do not sell, trade, or rent your personal information to third parties. We may share information with:</p>
<ul>
    <li>Service providers who assist in our operations</li>
    <li>Legal authorities when required by law</li>
    <li>Partners for specific programs (with your consent)</li>
</ul>

<h3>5. Data Security</h3>
<p>We implement appropriate security measures to protect your personal information. However, no method of transmission over the Internet is 100% secure.</p>

<h3>6. Your Rights</h3>
<p>You have the right to:</p>
<ul>
    <li>Access your personal information</li>
    <li>Request correction of inaccurate data</li>
    <li>Request deletion of your data</li>
    <li>Opt-out of marketing communications</li>
</ul>

<h3>7. Cookies</h3>
<p>We use cookies to enhance your experience on our website. You can control cookie settings through your browser preferences.</p>

<h3>8. Contact Us</h3>
<p>If you have questions about this Privacy Policy, please contact us at:</p>
<p>
Email: info@wbs-bd.org<br>
Phone: +880 1550-721313<br>
Address: House - 15 (5A), Road-08, Block -C, Bosila Garden City, Mohammadpur, Dhaka-1207
</p>

<h3>9. Changes to This Policy</h3>
<p>We may update this Privacy Policy from time to time. We will notify you of any changes by posting the new policy on this page.</p>',
    N'<h2>গোপনীয়তা নীতি</h2>
<p><strong>কার্যকর তারিখ:</strong> জানুয়ারি ২০২৫</p>

<h3>১. ভূমিকা</h3>
<p>ওয়ার্কিং বাংলাদেশ সোসাইটি (WBS) আপনার গোপনীয়তা রক্ষায় প্রতিশ্রুতিবদ্ধ। এই গোপনীয়তা নীতি ব্যাখ্যা করে কীভাবে আমরা আপনার তথ্য সংগ্রহ, ব্যবহার, প্রকাশ এবং সুরক্ষিত করি।</p>

<h3>২. আমরা যে তথ্য সংগ্রহ করি</h3>
<h4>২.১ ব্যক্তিগত তথ্য</h4>
<p>আমরা ব্যক্তিগতভাবে শনাক্তযোগ্য তথ্য সংগ্রহ করতে পারি যেমন:</p>
<ul>
    <li>নাম</li>
    <li>ইমেইল ঠিকানা</li>
    <li>ফোন নম্বর</li>
    <li>ডাক ঠিকানা</li>
    <li>পেমেন্ট তথ্য (দান করার সময়)</li>
</ul>

<h4>২.২ ব্যবহারের তথ্য</h4>
<p>আপনি আমাদের ওয়েবসাইট পরিদর্শন করলে আমরা স্বয়ংক্রিয়ভাবে কিছু তথ্য সংগ্রহ করি:</p>
<ul>
    <li>আইপি ঠিকানা</li>
    <li>ব্রাউজারের ধরন এবং সংস্করণ</li>
    <li>পরিদর্শন করা পৃষ্ঠাগুলি</li>
    <li>পরিদর্শনের সময় এবং তারিখ</li>
    <li>পৃষ্ঠায় ব্যয় করা সময়</li>
</ul>

<h3>৩. আমরা কীভাবে আপনার তথ্য ব্যবহার করি</h3>
<p>আমরা সংগৃহীত তথ্য ব্যবহার করি:</p>
<ul>
    <li>দান প্রক্রিয়া এবং রসিদ প্রদান করতে</li>
    <li>আমাদের প্রোগ্রাম সম্পর্কে নিউজলেটার এবং আপডেট পাঠাতে</li>
    <li>জিজ্ঞাসার উত্তর দিতে এবং সহায়তা প্রদান করতে</li>
    <li>আমাদের ওয়েবসাইট এবং সেবা উন্নত করতে</li>
    <li>আইনি বাধ্যবাধকতা মেনে চলতে</li>
</ul>

<h3>৪. তথ্য শেয়ারিং</h3>
<p>আমরা তৃতীয় পক্ষের কাছে আপনার ব্যক্তিগত তথ্য বিক্রয়, বিনিময় বা ভাড়া দিই না। আমরা তথ্য শেয়ার করতে পারি:</p>
<ul>
    <li>সেবা প্রদানকারীদের সাথে যারা আমাদের কার্যক্রমে সহায়তা করে</li>
    <li>আইনি কর্তৃপক্ষের সাথে যখন আইন দ্বারা প্রয়োজন</li>
    <li>নির্দিষ্ট প্রোগ্রামের জন্য অংশীদারদের সাথে (আপনার সম্মতিতে)</li>
</ul>

<h3>৫. ডেটা নিরাপত্তা</h3>
<p>আমরা আপনার ব্যক্তিগত তথ্য রক্ষা করতে উপযুক্ত সুরক্ষা ব্যবস্থা গ্রহণ করি। তবে, ইন্টারনেটে ট্রান্সমিশনের কোনো পদ্ধতি ১০০% নিরাপদ নয়।</p>

<h3>৬. আপনার অধিকার</h3>
<p>আপনার অধিকার আছে:</p>
<ul>
    <li>আপনার ব্যক্তিগত তথ্য অ্যাক্সেস করতে</li>
    <li>ভুল তথ্য সংশোধনের অনুরোধ করতে</li>
    <li>আপনার ডেটা মুছে ফেলার অনুরোধ করতে</li>
    <li>মার্কেটিং যোগাযোগ থেকে অপ্ট-আউট করতে</li>
</ul>

<h3>৭. কুকিজ</h3>
<p>আমরা আপনার ওয়েবসাইট অভিজ্ঞতা উন্নত করতে কুকিজ ব্যবহার করি। আপনি আপনার ব্রাউজার পছন্দের মাধ্যমে কুকি সেটিংস নিয়ন্ত্রণ করতে পারেন।</p>

<h3>৮. যোগাযোগ করুন</h3>
<p>এই গোপনীয়তা নীতি সম্পর্কে প্রশ্ন থাকলে, আমাদের সাথে যোগাযোগ করুন:</p>
<p>
ইমেইল: info@wbs-bd.org<br>
ফোন: +৮৮০ ১৫৫০-৭২১৩১৩<br>
ঠিকানা: হাউস - ১৫ (৫এ), রোড-০৮, ব্লক -সি, বসিলা গার্ডেন সিটি, মোহাম্মদপুর, ঢাকা-১২০৭
</p>

<h3>৯. এই নীতিতে পরিবর্তন</h3>
<p>আমরা সময়ে সময়ে এই গোপনীয়তা নীতি আপডেট করতে পারি। আমরা এই পৃষ্ঠায় নতুন নীতি পোস্ট করে যে কোনো পরিবর্তনের বিষয়ে আপনাকে অবহিত করব।</p>',
    N'Privacy Policy - WBS',
    N'Learn about how Working Bangladesh Society collects, uses, and protects your personal information.',
    1,
    1,
    GETDATE(),
    'System'
);

PRINT 'Privacy Policy page inserted with proper Unicode support.';
GO

-- Insert Terms & Conditions Page with proper Unicode support
INSERT INTO Pages (Title, TitleBn, Slug, Content, ContentBn, MetaTitle, MetaDescription, IsActive, ShowInFooter, CreatedAt, CreatedBy)
VALUES (
    N'Terms & Conditions',
    N'শর্তাবলী',
    'terms-conditions',
    N'<h2>Terms & Conditions</h2>
<p><strong>Effective Date:</strong> January 2025</p>

<h3>1. Acceptance of Terms</h3>
<p>By accessing and using the Working Bangladesh Society (WBS) website, you accept and agree to be bound by these Terms and Conditions.</p>

<h3>2. Use of Website</h3>
<h4>2.1 Permitted Use</h4>
<p>You may use our website for lawful purposes only. You agree not to:</p>
<ul>
    <li>Use the website in any way that violates applicable laws</li>
    <li>Attempt to gain unauthorized access to our systems</li>
    <li>Upload or transmit viruses or malicious code</li>
    <li>Engage in any activity that disrupts or interferes with the website</li>
</ul>

<h4>2.2 User Accounts</h4>
<p>If you create an account on our website, you are responsible for maintaining the confidentiality of your account credentials.</p>

<h3>3. Donations</h3>
<h4>3.1 Processing</h4>
<p>All donations are processed securely through authorized payment gateways. We do not store your complete payment card details.</p>

<h4>3.2 Tax Receipts</h4>
<p>Tax-deductible receipts will be issued for eligible donations in accordance with applicable tax laws.</p>

<h4>3.3 Refund Policy</h4>
<p>Donations are generally non-refundable. However, if you believe an error has occurred, please contact us within 30 days.</p>

<h3>4. Intellectual Property</h3>
<p>All content on this website, including text, images, logos, and graphics, is the property of WBS or its licensors and is protected by copyright laws.</p>

<h4>4.1 Use of Content</h4>
<p>You may view and download content for personal, non-commercial use only. Any other use requires our prior written permission.</p>

<h3>5. Third-Party Links</h3>
<p>Our website may contain links to third-party websites. We are not responsible for the content or privacy practices of these external sites.</p>

<h3>6. Disclaimer of Warranties</h3>
<p>The website is provided "as is" without warranties of any kind, either express or implied. We do not guarantee that:</p>
<ul>
    <li>The website will be uninterrupted or error-free</li>
    <li>Defects will be corrected</li>
    <li>The website is free of viruses or harmful components</li>
</ul>

<h3>7. Limitation of Liability</h3>
<p>To the fullest extent permitted by law, WBS shall not be liable for any indirect, incidental, special, or consequential damages arising from your use of the website.</p>

<h3>8. Privacy</h3>
<p>Your use of the website is also governed by our Privacy Policy. Please review our Privacy Policy to understand our practices.</p>

<h3>9. Volunteer and Event Registration</h3>
<p>By registering as a volunteer or for an event, you agree to:</p>
<ul>
    <li>Provide accurate information</li>
    <li>Follow WBS guidelines and code of conduct</li>
    <li>Attend scheduled activities as committed</li>
</ul>

<h3>10. Modifications</h3>
<p>We reserve the right to modify these Terms and Conditions at any time. Changes will be effective immediately upon posting on the website.</p>

<h3>11. Termination</h3>
<p>We may terminate or suspend your access to the website immediately, without prior notice, for any breach of these Terms.</p>

<h3>12. Governing Law</h3>
<p>These Terms shall be governed by and construed in accordance with the laws of Bangladesh.</p>

<h3>13. Contact Information</h3>
<p>For questions about these Terms and Conditions, please contact us at:</p>
<p>
Email: info@wbs-bd.org<br>
Phone: +880 1550-721313<br>
Address: House - 15 (5A), Road-08, Block -C, Bosila Garden City, Mohammadpur, Dhaka-1207
</p>',
    N'<h2>শর্তাবলী</h2>
<p><strong>কার্যকর তারিখ:</strong> জানুয়ারি ২০২৫</p>

<h3>১. শর্তাবলী গ্রহণ</h3>
<p>ওয়ার্কিং বাংলাদেশ সোসাইটি (WBS) ওয়েবসাইট অ্যাক্সেস এবং ব্যবহার করে, আপনি এই শর্তাবলী দ্বারা আবদ্ধ হতে সম্মত হন।</p>

<h3>২. ওয়েবসাইট ব্যবহার</h3>
<h4>২.১ অনুমোদিত ব্যবহার</h4>
<p>আপনি শুধুমাত্র আইনানুগ উদ্দেশ্যে আমাদের ওয়েবসাইট ব্যবহার করতে পারেন। আপনি সম্মত হন না:</p>
<ul>
    <li>প্রযোজ্য আইন লঙ্ঘন করে ওয়েবসাইট ব্যবহার করতে</li>
    <li>আমাদের সিস্টেমে অননুমোদিত অ্যাক্সেস পেতে চেষ্টা করতে</li>
    <li>ভাইরাস বা ক্ষতিকারক কোড আপলোড বা ট্রান্সমিট করতে</li>
    <li>ওয়েবসাইট ব্যাহত বা হস্তক্ষেপ করে এমন কোনো কার্যকলাপে জড়িত হতে</li>
</ul>

<h4>২.২ ব্যবহারকারী অ্যাকাউন্ট</h4>
<p>আপনি যদি আমাদের ওয়েবসাইটে একটি অ্যাকাউন্ট তৈরি করেন, তবে আপনার অ্যাকাউন্ট শংসাপত্রের গোপনীয়তা বজায় রাখার জন্য আপনি দায়ী।</p>

<h3>৩. দান</h3>
<h4>৩.১ প্রক্রিয়াকরণ</h4>
<p>সমস্ত দান অনুমোদিত পেমেন্ট গেটওয়ের মাধ্যমে নিরাপদভাবে প্রক্রিয়া করা হয়। আমরা আপনার সম্পূর্ণ পেমেন্ট কার্ডের বিবরণ সংরক্ষণ করি না।</p>

<h4>৩.২ কর রসিদ</h4>
<p>প্রযোজ্য কর আইন অনুসারে যোগ্য দানের জন্য কর-কর্তনযোগ্য রসিদ জারি করা হবে।</p>

<h4>৩.৩ ফেরত নীতি</h4>
<p>দান সাধারণত ফেরতযোগ্য নয়। তবে, আপনি যদি মনে করেন একটি ত্রুটি ঘটেছে, অনুগ্রহ করে ৩০ দিনের মধ্যে আমাদের সাথে যোগাযোগ করুন।</p>

<h3>৪. মেধা সম্পত্তি</h3>
<p>এই ওয়েবসাইটের সমস্ত বিষয়বস্তু, টেক্সট, ছবি, লোগো এবং গ্রাফিক্স সহ, WBS বা এর লাইসেন্সদাতাদের সম্পত্তি এবং কপিরাইট আইন দ্বারা সুরক্ষিত।</p>

<h4>৪.১ বিষয়বস্তু ব্যবহার</h4>
<p>আপনি শুধুমাত্র ব্যক্তিগত, অ-বাণিজ্যিক ব্যবহারের জন্য বিষয়বস্তু দেখতে এবং ডাউনলোড করতে পারেন। অন্য কোনো ব্যবহারের জন্য আমাদের পূর্ব লিখিত অনুমতি প্রয়োজন।</p>

<h3>৫. তৃতীয় পক্ষের লিংক</h3>
<p>আমাদের ওয়েবসাইটে তৃতীয় পক্ষের ওয়েবসাইটের লিংক থাকতে পারে। আমরা এই বাহ্যিক সাইটের বিষয়বস্তু বা গোপনীয়তা অনুশীলনের জন্য দায়ী নই।</p>

<h3>৬. ওয়্যারেন্টি অস্বীকৃতি</h3>
<p>ওয়েবসাইটটি "যেমন আছে" প্রদান করা হয় কোনো ধরনের ওয়্যারেন্টি ছাড়াই। আমরা গ্যারান্টি দিই না যে:</p>
<ul>
    <li>ওয়েবসাইট নিরবচ্ছিন্ন বা ত্রুটি-মুক্ত হবে</li>
    <li>ত্রুটি সংশোধন করা হবে</li>
    <li>ওয়েবসাইট ভাইরাস বা ক্ষতিকারক উপাদান মুক্ত</li>
</ul>

<h3>৭. দায়বদ্ধতার সীমাবদ্ধতা</h3>
<p>আইন দ্বারা অনুমোদিত সম্পূর্ণ পরিমাণে, WBS ওয়েবসাইট ব্যবহার থেকে উদ্ভূত কোনো পরোক্ষ, আনুষঙ্গিক, বিশেষ বা ফলস্বরূপ ক্ষতির জন্য দায়ী থাকবে না।</p>

<h3>৮. গোপনীয়তা</h3>
<p>আপনার ওয়েবসাইট ব্যবহার আমাদের গোপনীয়তা নীতি দ্বারাও নিয়ন্ত্রিত হয়। আমাদের অনুশীলন বুঝতে আমাদের গোপনীয়তা নীতি পর্যালোচনা করুন।</p>

<h3>৯. স্বেচ্ছাসেবক এবং ইভেন্ট নিবন্ধন</h3>
<p>স্বেচ্ছাসেবক হিসাবে বা একটি ইভেন্টের জন্য নিবন্ধন করে, আপনি সম্মত হন:</p>
<ul>
    <li>সঠিক তথ্য প্রদান করতে</li>
    <li>WBS নির্দেশিকা এবং আচরণবিধি অনুসরণ করতে</li>
    <li>প্রতিশ্রুতিবদ্ধ হিসাবে নির্ধারিত কার্যক্রমে উপস্থিত থাকতে</li>
</ul>

<h3>১০. পরিবর্তন</h3>
<p>আমরা যেকোনো সময় এই শর্তাবলী পরিবর্তন করার অধিকার সংরক্ষণ করি। পরিবর্তনগুলি ওয়েবসাইটে পোস্ট করার সাথে সাথে কার্যকর হবে।</p>

<h3>১১. সমাপ্তি</h3>
<p>আমরা এই শর্তাবলীর যেকোনো লঙ্ঘনের জন্য পূর্ব বিজ্ঞপ্তি ছাড়াই অবিলম্বে ওয়েবসাইটে আপনার অ্যাক্সেস বন্ধ বা স্থগিত করতে পারি।</p>

<h3>১২. প্রযোজ্য আইন</h3>
<p>এই শর্তাবলী বাংলাদেশের আইন অনুসারে পরিচালিত এবং ব্যাখ্যা করা হবে।</p>

<h3>১৩. যোগাযোগের তথ্য</h3>
<p>এই শর্তাবলী সম্পর্কে প্রশ্নের জন্য, আমাদের সাথে যোগাযোগ করুন:</p>
<p>
ইমেইল: info@wbs-bd.org<br>
ফোন: +৮৮০ ১৫৫০-৭২১৩১৩<br>
ঠিকানা: হাউস - ১৫ (৫এ), রোড-০৮, ব্লক -সি, বসিলা গার্ডেন সিটি, মোহাম্মদপুর, ঢাকা-১২০৭
</p>',
    N'Terms & Conditions - WBS',
    N'Read the terms and conditions for using Working Bangladesh Society website and services.',
    1,
    1,
    GETDATE(),
    'System'
);

PRINT 'Terms & Conditions page inserted with proper Unicode support.';
GO

-- Verify the inserted pages
SELECT 
    Id, 
    Title, 
    TitleBn, 
    Slug, 
    CAST(LEFT(ContentBn, 100) AS NVARCHAR(100)) AS ContentBnPreview,
    IsActive, 
    ShowInFooter, 
    CreatedAt
FROM Pages
WHERE Slug IN ('privacy-policy', 'terms-conditions')
ORDER BY Slug;
GO

PRINT '=====================================================';
PRINT 'Script completed successfully with Unicode support!';
PRINT 'Privacy Policy URL: /page/privacy-policy';
PRINT 'Terms & Conditions URL: /page/terms-conditions';
PRINT '=====================================================';
PRINT 'NOTE: All text is stored with N prefix for proper Unicode support';
