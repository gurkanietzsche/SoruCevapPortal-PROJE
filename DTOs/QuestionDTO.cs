using System;
using System.Collections.Generic;

namespace SoruCevapPortal.DTOs
{
    // QuestionDTO sınıfı
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int ViewCount { get; set; }
        public UserDTO User { get; set; }
        public List<TagDTO> Tags { get; set; }
        public int VoteCount { get; set; }
        public int AnswerCount { get; set; }
    }

    // QuestionCreateDTO sınıfı - Aynı namespace altında
    public class QuestionCreateDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }

        // Var olan etiketlerin ID'leri (opsiyonel)
        public List<int> TagIds { get; set; } // Nullable operatörü (?) kaldırıldı

        // Yeni oluşturulacak etiket adları (opsiyonel) 
        public List<string> Tags { get; set; } // Nullable operatörü (?) kaldırıldı
    }

    // QuestionUpdateDTO sınıfı - Aynı namespace altında
    public class QuestionUpdateDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }

        // Var olan etiketlerin ID'leri (opsiyonel)
        public List<int> TagIds { get; set; } // Nullable operatörü (?) kaldırıldı

        // Yeni oluşturulacak etiket adları (opsiyonel)
        public List<string> Tags { get; set; } // Nullable operatörü (?) kaldırıldı
    }
}