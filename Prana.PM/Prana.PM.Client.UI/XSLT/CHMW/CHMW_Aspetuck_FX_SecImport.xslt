<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here"
	xmlns:my1="put-your-namespace-uri-here"
	xmlns:my2="put-your-namespace-uri-here"
	xmlns:my5="put-your-namespace-uri-here"
	xmlns:my3="put-your-namespace-uri-here">

	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>

	<msxsl:script language="C#" implements-prefix="my5">
		<msxsl:assembly name="System.Data"/>
		<msxsl:using namespace="System.Data"/>
		<msxsl:assembly name="System.IO"/>
		<msxsl:using namespace="System.IO"/>

		public String DateCheck(String trade, String settlement)
		{
		DateTime tradeDate = Convert.ToDateTime(trade);
		DateTime settlementDate = Convert.ToDateTime(settlement);
		string mainPath = Directory.GetCurrentDirectory();
		string mappingPath = Path.GetFullPath(Path.Combine(mainPath, "Client Release/MappingFiles/ReconMappingXML/HolidayMapping.xml"));
		DataSet ds = new DataSet();

		ds.ReadXml(mappingPath);
		int count = 0;

		for (DateTime dt = tradeDate.AddDays(1); DateTime.Compare(dt, settlementDate) &lt;= 0; dt = dt.AddDays(1))
		{
		if (!(dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday))
		{
		bool flag = false;
		for (int i = 0; i &lt; ds.Tables[1].Rows.Count; i++)
		{
		if(DateTime.Compare(dt,Convert.ToDateTime(ds.Tables[1].Rows[i][1].ToString())) == 0)
		{
		flag = true;
		}
		}
		if (!flag)
		{
		count++;
		}
		}
		}
		if (count &lt; 3)
		{
		return "Spot";
		}
		else
		{
		return "Forward";
		}
		}
	</msxsl:script>

	

	<msxsl:script language="C#" implements-prefix="my1">
		public double DateDiff(DateTime StartDate, DateTime EndDate)
		{

		return (EndDate - StartDate).TotalDays;
		}
	</msxsl:script>

	


  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="COL1">
          <xsl:choose>
            <xsl:when test="substring(COL1,1,3)='USD'">
              <xsl:value-of select="concat(substring(COL1,4,3),substring(COL1,1,3))"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="normalize-space(COL1)"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

    

        <xsl:if test="COL16 and contains(COL26,'FX')">

          <PositionMaster>

            <xsl:variable name="varExpMon">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(COL24,'/'))=1">
                  <xsl:value-of select="concat('0',substring-before(COL24,'/'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before(COL24,'/')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varExpDay">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(substring-after(COL24,'/'),'/'))=1">
                  <xsl:value-of select="concat('0',substring-before(substring-after(COL24,'/'),'/'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before(substring-after(COL24,'/'),'/')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varMonth">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(COL24,'/'))=1">
                  <xsl:value-of select="concat('0',substring-before(COL24,'/'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before(COL24,'/')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="ExpirationDate">
              <!--<xsl:value-of select="concat(substring-after(substring-after(COL10,'/'),'/'),$varExpMon,$varExpDay)"/>-->
              <xsl:value-of select="concat($varMonth,'/',$varExpDay,'/',substring(substring-after(substring-after(COL24,'/'),'/'),3))"/>
            </xsl:variable>


			
			
			

			  <xsl:variable name="PositionSettlementDate">
				  <xsl:choose>
					  <xsl:when test="contains(COL26,'FX')">
						  <xsl:value-of select="concat(substring(COL24,5,2),'/',substring(COL24,7,2),'/',substring(COL24,3,2))"/>
					  </xsl:when>
				  </xsl:choose>
			  </xsl:variable>

			  
			  	<xsl:variable name="SettleMentDate">
							<xsl:value-of select ="concat(substring(COL24,5,2),'/',substring(COL24,7,2),'/',substring(COL24,1,4))"/>
						</xsl:variable>


			  <xsl:variable name="PositionSettlementDate1">
				  <xsl:value-of select="concat(substring(COL24,5,2),'/',substring(COL24,7,2),'/',substring(COL24,3,2))"/>
			  </xsl:variable>


			  <xsl:variable name="TradeDate">
				  <xsl:value-of select ="concat(substring(COL22,5,2),'/',substring(COL22,7,2),'/',substring(COL22,1,4))"/>
			  </xsl:variable>


			  <xsl:variable name="Final">
				  <xsl:value-of select="my5:DateCheck($TradeDate,$SettleMentDate)"/>
			  </xsl:variable>

			  <!--<xsl:variable name="SFinal">

				  <xsl:value-of select="concat(substring-before(substring-after($Final,'-'),'-'),'/',substring-before(substring-after(substring-after($Final,'-'),'-'),'T'),'/',substring-before($Final,'-'))"/>
			  </xsl:variable>-->


			 

			 

			 

			  <xsl:variable name ="DIFFDate">
							 <xsl:value-of select="my5:DateCheck($TradeDate,$SettleMentDate)"/>
						</xsl:variable>



			 


			  <xsl:variable name ="varSymbolFX">
				  <xsl:choose>
					  <xsl:when test="contains(COL26,'FX')">
						  <xsl:choose>

							  <xsl:when test="COL5=1">
								  <xsl:choose>
									  <xsl:when test="$Final='Forward'">
										  <xsl:value-of select="concat(substring-before(COL6,'.'),'/','USD',' ','N',' ',$PositionSettlementDate1,' ','CURNCY')"/>
									  </xsl:when>
									  <xsl:otherwise>
										  <xsl:value-of select="concat(substring-before(COL6,'.'),'USD CURNCY')"/>
									  </xsl:otherwise>
								  </xsl:choose>
							  </xsl:when>
							  <xsl:when test="COL5!=1">
								  <xsl:choose>
									  <xsl:when test="$Final='Forward'">
										  <xsl:value-of select="concat(substring-after(COL6,'.'),'/','USD',' ','N',' ',$PositionSettlementDate1,' ','CURNCY')"/>
									  </xsl:when>
									  <xsl:otherwise>
										  <xsl:value-of select="concat(substring-after(COL6,'.'),'USD CURNCY')"/>
									  </xsl:otherwise>
								  </xsl:choose>
							  </xsl:when>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test="COL3='USD' and COL4='STK'">
								  <xsl:value-of select="concat(COL6,' ','US EQUITY')"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select="''"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:otherwise>
				  </xsl:choose>

			  </xsl:variable>



			  <TickerSymbol>
              <xsl:value-of select="$varSymbolFX"/>
            </TickerSymbol>

			  <xsl:variable name="AUEC">
				  <xsl:choose>
					  <xsl:when test="contains(COL26,'FX')">
						  <xsl:choose>
							  <xsl:when test="$Final='Forward'">
								  <xsl:value-of select="'FX-FXFWD'"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select="'FX-FX'"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:when>
					 
				  </xsl:choose>
			  </xsl:variable>

            <AUEC>
              <xsl:value-of select="$AUEC"/>
            </AUEC>

			  <xsl:variable name="Currency">
				  <xsl:choose>
					  <xsl:when test="contains(COL26,'FX')">
						  <xsl:choose>
							  <xsl:when test="$Final='Forward'">
								  <xsl:value-of select="'MUL'"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select="'USD'"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:otherwise>
						  <!--<xsl:choose>-->
							  <!--<xsl:when test="contains(COL26,'FX')">
								  <xsl:value-of select="'USD'"/>
							  </xsl:when>
							  <xsl:otherwise>-->
								  <xsl:value-of select="'MUL'"/>
							  <!--</xsl:otherwise>-->
						  <!--</xsl:choose>-->
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

            <Currency>
				<!--<xsl:choose>
					
					<xsl:when test="contains(COL26,'FX')">
						<xsl:value-of select="'USD'"/>
					</xsl:when>
					
					<xsl:when test="$DIFFDate &gt; 2">
						<xsl:value-of select="'MUL'"/>
					</xsl:when>
					
					<xsl:otherwise>
						<xsl:value-of select="'MUL'"/>
					</xsl:otherwise>
				</xsl:choose>-->
				<xsl:value-of select="$Currency"/>
            </Currency>

            <LongName>
              <xsl:value-of select="$varSymbolFX"/>
            </LongName>

            <Multiplier>
              <xsl:value-of select="1"/>
            </Multiplier>

            <UnderLyingSymbol>
              <xsl:value-of select="$varSymbolFX"/>
            </UnderLyingSymbol>

            <ExpirationDate>
              <xsl:value-of select="$SettleMentDate"/>
            </ExpirationDate>

            <SettlementDate>
              <xsl:value-of select="$SettleMentDate"/>
            </SettlementDate>
			
			<FirstTradeDate>
              <xsl:value-of select="''"/>
            </FirstTradeDate>

            <BloombergSymbol>
              <xsl:value-of select="$varSymbolFX"/>
            </BloombergSymbol>

			  <xsl:variable name="LeadCurrency">
				  <xsl:choose>
					  <xsl:when test="COL5=1">
						  <xsl:value-of select="substring-before(COL6,'.')"/>
					  </xsl:when>
					  <xsl:when test="COL5!=1">
						  <xsl:value-of select="substring-after(COL6,'.')"/>
					  </xsl:when>
				  </xsl:choose>
			  </xsl:variable>

            <LeadCurrency>
              <xsl:value-of select="$LeadCurrency"/>
            </LeadCurrency>

            <VsCurrency>
              <xsl:value-of select="'USD'"/>
            </VsCurrency>

            <Comments>
              <xsl:value-of select="'Created by sec master import'"/>
            </Comments>

            <SName>
              <xsl:value-of select="$varSymbolFX"/>
            </SName>

            <Description>
              <xsl:value-of select="$varSymbolFX"/>
            </Description>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>



