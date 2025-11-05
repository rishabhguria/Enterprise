<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

  <xsl:output method="xml" indent="yes"/>

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

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'$',''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">


        <xsl:variable name="NetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL5"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($NetPosition) and (COL2='038CACKK7' or COL2='038CACKJ0')">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL8"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <!--<xsl:variable name="Asset">
              <xsl:choose>                

                <xsl:when test="contains(COL11,'SWAP') or contains(COL11,'Swap')">
                  <xsl:value-of select="'Swap'"/>
                </xsl:when>

                <xsl:when test="contains(COL11,'Option')">
                  <xsl:value-of select="'Option'"/>
                </xsl:when>

                <xsl:when test="contains(COL11,'Forward')">
                  <xsl:value-of select="'Forward'"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>-->



            <xsl:variable name="Symbol" select="COL3"/>
            <Symbol>


              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$Symbol!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>


                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>

            </Symbol>

            <SEDOL>

              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$Symbol!='*'">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>


                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

            <xsl:variable name="PB_FUND_NAME" select="COL2"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>

                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>

              </xsl:choose>
            </AccountName>



            <NetPosition>
              <xsl:choose>
                <xsl:when test="$NetPosition&gt; 0">
                  <xsl:value-of select="$NetPosition"/>
                </xsl:when>
                <xsl:when test="$NetPosition&lt; 0">
                  <xsl:value-of select="$NetPosition* (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>






			  <xsl:variable name="CostBasis" select="COL6"/>
            <CostBasis>
              <xsl:choose>
                <xsl:when test="$CostBasis &gt; 0">
                  <xsl:value-of select="$CostBasis"/>

                </xsl:when>
                <xsl:when test="$CostBasis &lt; 0">
                  <xsl:value-of select="$CostBasis * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </CostBasis>

            <xsl:variable name="Side" select="COL4"/>

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$Side='BUY'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$Side='SELL'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="$Side='BUY TO CLOSE'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:when test="$Side='SELL SHORT'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>



            </SideTagValue>

            <!--<SEDOL>
              <xsl:value-of select="COL9" />
            </SEDOL>-->

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

			  <xsl:variable name="varMDate">
				  <xsl:value-of select="substring-before(substring-after(COL13,'-'),'-')"/>
			  </xsl:variable>
			  <xsl:variable name="varDDate">
				  <xsl:value-of select="substring-before(COL13,'-')"/>
			  </xsl:variable>
			  <xsl:variable name="varYear">
				  <xsl:value-of select="substring-after(substring-after(COL13,'-'),'-')"/>
			  </xsl:variable>

            <PositionStartDate>
				<!--<xsl:value-of select="concat($varMDate,'/',$varDDate,'/',$varYear)"/>-->
				<xsl:value-of select="COL9"/>
            </PositionStartDate>


			  <!--<xsl:variable name="varSMDate">
				  <xsl:value-of select="substring-before(substring-after(COL10,'-'),'-')"/>
			  </xsl:variable>
			  <xsl:variable name="varSDDate">
				  <xsl:value-of select="substring-before(COL10,'-')"/>
			  </xsl:variable>
			  <xsl:variable name="varSYear">
				  <xsl:value-of select="substring-after(substring-after(COL10,'-'),'-')"/>
			  </xsl:variable>-->

			  <PositionSettlementDate>
				  <!--<xsl:value-of select="concat($varSMDate,'/',$varSDDate,'/',$varSYear)"/>-->
				  <xsl:value-of select="COL10"/>
			  </PositionSettlementDate>




			  <xsl:variable name="PB_COUNTER_PARTY" select="'REBL'"/>

            <xsl:variable name="PRANA_COUNTER_PARTY">
              <xsl:value-of select ="document('../ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PBBroker=$PB_COUNTER_PARTY]/@PranaBrokerCode"/>
            </xsl:variable>

            <CounterPartyID>
              <xsl:choose>

                <xsl:when test ="number($PRANA_COUNTER_PARTY) ">
                  <xsl:value-of select ="$PRANA_COUNTER_PARTY"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="56"/>
                </xsl:otherwise>

              </xsl:choose>
            </CounterPartyID>





          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>