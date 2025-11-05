<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:output method="xml" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime"/>

    <xsl:variable name="varDate">
      <xsl:choose>
        <xsl:when test="contains($DateTime,'/')">
          <xsl:choose>
            <xsl:when test="string-length(substring-before(substring-after(normalize-space($DateTime),'/'),'/'))=1">
              <xsl:value-of select="concat(0,substring-before(substring-after($DateTime,'/'),'/'))"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring-before(substring-after($DateTime,'/'),'/')"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="contains($DateTime,'-')">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(substring-after(normalize-space($DateTime),'-'),'-'))=1">
                  <xsl:value-of select="concat(0,substring-before(substring-after($DateTime,'-'),'-'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before(substring-after($DateTime,'-'),'-')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varMonth">
      <xsl:choose>
        <xsl:when test="contains($DateTime,'/')">
          <xsl:choose>
            <xsl:when test="string-length(substring-before(normalize-space($DateTime),'/'))=1">
              <xsl:value-of select="concat(0,substring-before($DateTime,'/'))"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring-before($DateTime,'/')"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="contains($DateTime,'-')">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(normalize-space($DateTime),'-'))=1">
                  <xsl:value-of select="concat(0,substring-before($DateTime,'-'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before($DateTime,'-')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varYear">
      <xsl:choose>
        <xsl:when test="contains($DateTime,'/')">
          <xsl:value-of select="substring-after(substring-after($DateTime,'/'),'/')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="contains($DateTime,'-')">
              <xsl:value-of select="substring-after(substring-after($DateTime,'-'),'-')"/>
            </xsl:when>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    
    <xsl:value-of select="$varMonth"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$varDate"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$varYear"/>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="NetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL8)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($NetPosition) and COL12='5'" >
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL16)"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL36)"/>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:choose>
                <xsl:when test="substring(COL25,1,1)='S'">
                  <xsl:value-of select="normalize-space(COL27)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test ="$varSEDOL!='' and $varSEDOL!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test ="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <SEDOL>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test ="$varSEDOL!='' and $varSEDOL!='*'">
                  <xsl:value-of select="$varSEDOL"/>
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
                <xsl:when test="$NetPosition &gt; 0">
                  <xsl:value-of select="$NetPosition"/>
                </xsl:when>
                
                <xsl:when test="$NetPosition &lt; 0">
                  <xsl:value-of select="$NetPosition * (-1)"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="var1">
              <xsl:choose>
                <xsl:when test="$NetPosition &gt; 0">
                  <xsl:value-of select="$NetPosition"/>
                </xsl:when>
                
                <xsl:when test="$NetPosition &lt; 0">
                  <xsl:value-of select="$NetPosition * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="var2">
              <xsl:choose>
                <xsl:when test="contains(COL23,'-')">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="normalize-space(COL23)"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="normalize-space(COL23)"/>
                  </xsl:call-template>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="CostBasis">
              <xsl:value-of select="$var2 div $var1"/>
            </xsl:variable>
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

            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL18"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
                
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission * (-1)"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <xsl:variable name="varSecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL22"/>
              </xsl:call-template>
            </xsl:variable>
            <SecFee>
              <xsl:choose>
                <xsl:when test="$varSecFee &gt; 0">
                  <xsl:value-of select="$varSecFee"/>
                </xsl:when>
                
                <xsl:when test="$varSecFee &lt; 0">
                  <xsl:value-of select="$varSecFee * (-1)"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>

            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL13)"/>
            </xsl:variable>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varSide='Purchases'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>

                <xsl:when test="$varSide='Sales'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <PositionStartDate>
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="COL11"/>
              </xsl:call-template>
            </PositionStartDate>

            <PositionSettlementDate>
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="COL10"/>
              </xsl:call-template>
            </PositionSettlementDate>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>