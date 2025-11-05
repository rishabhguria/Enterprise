<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="tempSymbolCode_SUFFIX">
    <xsl:param name="paramSymbolCode_SUFFIX"/>
    <xsl:choose>
      <xsl:when test ="$paramSymbolCode_SUFFIX = 'AU'">
        <xsl:value-of select ="'ASX'"/>
      </xsl:when>
      <xsl:when test ="$paramSymbolCode_SUFFIX = 'JP'">
        <xsl:value-of select ="'TSE'"/>
      </xsl:when>
      <xsl:when test ="$paramSymbolCode_SUFFIX = 'HK'">
        <xsl:value-of select ="'HKG'"/>
      </xsl:when>
      <xsl:when test ="$paramSymbolCode_SUFFIX = 'KS'">
        <xsl:value-of select ="'KSE'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="noofzeros">
    <xsl:param name="count"/>
    <xsl:if test="$count > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofzeros">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test ="COL1 != 'assetKey'">
          <PositionMaster>

            <!--<xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL7"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <AccountName>
                  <xsl:value-of select='$PB_FUND_NAME'/>
                </AccountName>
              </xsl:when>
              <xsl:otherwise>
                <AccountName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </AccountName>
              </xsl:otherwise>
            </xsl:choose >-->


            <AccountName>
              <xsl:value-of select="''"/>
            </AccountName>
            
            <!--<xsl:choose>
              <xsl:when  test="number(COL14)">
                <CostBasis>
                  <xsl:value-of select="COL14"/>
                </CostBasis>
              </xsl:when >
              <xsl:otherwise>
                <CostBasis>
                  <xsl:value-of select="0"/>
                </CostBasis>
              </xsl:otherwise>
            </xsl:choose >-->

            <CostBasis>
              <xsl:value-of select="10"/>
            </CostBasis>


            <!--<xsl:choose>
              <xsl:when  test="number(COL13) &lt; 0 ">
                <NetPosition>
                  <xsl:value-of select="COL13 * -1"/>
                </NetPosition>
              </xsl:when>
              <xsl:when  test="number(COL13) &gt; 0">
                <NetPosition>
                  <xsl:value-of select="number(COL13)"/>
                </NetPosition>
              </xsl:when>
              <xsl:otherwise>
                <NetPosition>
                  <xsl:value-of select="0"/>
                </NetPosition>
              </xsl:otherwise>
            </xsl:choose>-->

            <!--<NetPosition>
              <xsl:value-of select="10"/>
            </NetPosition>-->

            <PositionStartDate>
              <xsl:value-of select="''"/>
            </PositionStartDate>

            <NetPosition>
              <xsl:value-of select="9"/>
            </NetPosition>



            <SideTagValue>
              <xsl:value-of select="'1'"/>
            </SideTagValue>


           

            <xsl:variable name = "PB_SYMBOL" >
              <xsl:value-of select="COL2"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='SANSATO']/SymbolData[@PBCompanyName=$PB_SYMBOL]/@PranaSymbol"/>
            </xsl:variable>


            <!--<xsl:variable name = "varSymbol_PostFIX" >
						<xsl:call-template name="tempSymbolCode_POSTFIX">
						  <xsl:with-param name="paramSymbolCode_POSTFIX" select="normalize-space($varSymbolCode)" />
						</xsl:call-template>
						 </xsl:variable>-->

            <xsl:variable name ="varSymbolCode">
              <xsl:value-of select ="substring-after(COL2,' ')"/>
            </xsl:variable>
            <xsl:variable name="TickerSuffixCode">
              <xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name='SANSATO']/SymbolData[@PBSuffixCode=$varSymbolCode]/@TickerSuffixCode"/>
            </xsl:variable>

            <xsl:variable name ="varHKGSymbol">
              <xsl:call-template name="noofzeros">
                <xsl:with-param name="count" select="(4) - string-length(substring-before(COL2,' '))" />
              </xsl:call-template>
            </xsl:variable>


            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL"/>
                </xsl:when>
                <xsl:when test ="$varSymbolCode = 'AU'">
                  <xsl:value-of select="concat(substring-before(COL2,' '),'-',$TickerSuffixCode)"/>
                </xsl:when>
                <xsl:when test ="$varSymbolCode = 'HK'">
                  <xsl:value-of select="concat($varHKGSymbol,substring-before(COL2,' '),'-',$TickerSuffixCode)"/>
                </xsl:when>
                <xsl:when test ="$varSymbolCode = 'IJ'">
                  <xsl:value-of select="concat(substring-before(COL2,' '),'-',$TickerSuffixCode)"/>
                </xsl:when>
                <xsl:when test ="$varSymbolCode = 'IN'">
                  <xsl:value-of select="concat(substring-before(COL2,' '),'.',$TickerSuffixCode)"/>
                </xsl:when>
                <xsl:when test ="$varSymbolCode = 'JP'">
                  <xsl:value-of select="concat(substring-before(COL2,' '),'-',$TickerSuffixCode)"/>
                </xsl:when>
                <xsl:when test ="$varSymbolCode = 'KS'">
                  <xsl:value-of select="concat(substring-before(COL2,' '),'-',$TickerSuffixCode)"/>
                </xsl:when>
                <xsl:when test ="$varSymbolCode = 'MK'">
                  <xsl:value-of select="concat(substring-before(COL2,' '),'-',$TickerSuffixCode)"/>
                </xsl:when>
                <xsl:when test ="$varSymbolCode = 'NZ'">
                  <xsl:value-of select="concat(substring-before(COL2,' '),'-',$TickerSuffixCode)"/>
                </xsl:when>
                <xsl:when test ="$varSymbolCode = 'SP'">
                  <xsl:value-of select="concat(substring-before(COL2,' '),'-',$TickerSuffixCode)"/>
                </xsl:when>
                <xsl:when test ="$varSymbolCode = 'TB'">
                  <xsl:value-of select="concat(substring-before(COL2,' '),'-',$TickerSuffixCode)"/>
                </xsl:when>
                <xsl:when test ="$varSymbolCode = 'TT'">
                  <xsl:value-of select="concat(substring-before(COL2,' '),'-',$TickerSuffixCode)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL2"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <PBSymbol>
              <xsl:value-of select ="COL3"/>
            </PBSymbol>

            <Description>
              <xsl:value-of select ="COL2"/>
            </Description>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>


