using System;
using System.Collections.Generic;
using System.Text;

namespace ArchApp.Models
{
    public class USD
    {
        public int available_in_cents { get; set; }
        public int pending_in_cents { get; set; }
        public int reserved_in_cents { get; set; }
    }

    public class CAD
    {
        public int available_in_cents { get; set; }
        public int pending_in_cents { get; set; }
        public int reserved_in_cents { get; set; }
    }

    public class AUD
    {
        public int available_in_cents { get; set; }
        public int pending_in_cents { get; set; }
        public int reserved_in_cents { get; set; }
    }

    public class PRO
    {
        public int available_in_cents { get; set; }
        public int pending_in_cents { get; set; }
        public int reserved_in_cents { get; set; }
    }

    public class BND
    {
        public int available_in_cents { get; set; }
        public int pending_in_cents { get; set; }
        public int reserved_in_cents { get; set; }
    }

    public class Fundsbycurrency
    {
        public USD USD { get; set; }
        public CAD CAD { get; set; }
        public AUD AUD { get; set; }
        public PRO PRO { get; set; }
        public BND BND { get; set; }
    }

    public class Funds
    {
        public Info info { get; set; }
        public Fundsbycurrency fundsbycurrency { get; set; }
    }
}
