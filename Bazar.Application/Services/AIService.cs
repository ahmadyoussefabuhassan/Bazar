using Bazar.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Bazar.Application.Services
{
    public class AIService : IAIService
    {
        // الكونستركتور يستقبل المفتاح (شكلياً) لكي لا نضطر لتعديل Program.cs
        public AIService(string apiKey)
        {
        }

        public async Task<string> SendMessageAsync(string message)
        {
            try
            {
                // محاكاة تأخير بسيط (نص ثانية) ليبدو وكأنه يفكر مثل الذكاء الاصطناعي الحقيقي
                await Task.Delay(500);

                if (string.IsNullOrWhiteSpace(message))
                    return "مرحباً! كيف يمكنني مساعدتك؟";

                // توحيد النص ليسهل البحث فيه
                var msg = message.Trim().ToLower();

                // --- منطق الردود الذكية (Rule-Based) ---

                // 1. الترحيب
                if (msg.Contains("مرحبا") || msg.Contains("هلا") || msg.Contains("سلام") || msg.Contains("هاي"))
                {
                    return "أهلاً بك في تطبيق بازار! 🌹 يسعدني مساعدتك. يمكنك سؤالي عن المنتجات، الأسعار، أو طريقة الشراء.";
                }

                // 2. الأسعار
                if (msg.Contains("سعر") || msg.Contains("اسعار") || msg.Contains("بكام") || msg.Contains("كم"))
                {
                    return "الأسعار في بازار متنوعة جداً! 💰 يمكنك معرفة سعر أي منتج بالضغط عليه لرؤية التفاصيل. إذا كنت تبحث عن شيء محدد، أخبرني باسمه.";
                }

                // 3. طريقة الشراء
                if (msg.Contains("شراء") || msg.Contains("اشتري") || msg.Contains("طلب") || msg.Contains("توصيل"))
                {
                    return "عملية الشراء سهلة! 🛒\n1. اختر المنتج الذي يعجبك.\n2. تواصل مع البائع عبر الرقم الموجود في الصفحة.\n3. اتفق معه على التسليم.";
                }

                // 4. الحساب وتسجيل الدخول
                if (msg.Contains("حساب") || msg.Contains("تسجيل") || msg.Contains("دخول") || msg.Contains("كلمة السر"))
                {
                    return "يمكنك إدارة حسابك من صفحة 'ملفي الشخصي'. تأكد من تسجيل الدخول لتتمكن من إضافة منتجاتك الخاصة.";
                }

                // 5. الشكر
                if (msg.Contains("شكرا") || msg.Contains("يسلمو") || msg.Contains("ثانكس") || msg.Contains("يعطيك العافية"))
                {
                    return "على الرحب والسعة! أنا في خدمتك دائماً. بالتوفيق! 😊";
                }

                // 6. الهوية
                if (msg.Contains("من انت") || msg.Contains("اسمك") || msg.Contains("بوت"))
                {
                    return "أنا المساعد الذكي الخاص بتطبيق بازار 🤖. مهمتي تسهيل تجربتك في التطبيق.";
                }

                // 7. البحث عن منتجات (كتب، إلكترونيات...)
                if (msg.Contains("كتاب") || msg.Contains("كتب"))
                {
                    return "لدينا قسم رائع للكتب! 📚 اذهب إلى التصنيفات واختر 'كتب' لتجد ما تبحث عنه.";
                }

                if (msg.Contains("لابتوب") || msg.Contains("موبايل") || msg.Contains("هاتف"))
                {
                    return "الإلكترونيات من أكثر الأقسام طلباً! 💻📱 تصفح قسم الإلكترونيات لترى أحدث العروض.";
                }

                // --- الرد الافتراضي (إذا لم يفهم السؤال) ---
                return "عذراً، لم أفهم سؤالك تماماً. 🤔\nيمكنك سؤالي مثلاً:\n- كيف أشتري منتجاً؟\n- كيف أنشئ حساباً؟\n- ما هي المنتجات المتوفرة؟";
            }
            catch (Exception)
            {
                return "عذراً، حدث خطأ بسيط. يرجى المحاولة مرة أخرى.";
            }
        }
    }
}