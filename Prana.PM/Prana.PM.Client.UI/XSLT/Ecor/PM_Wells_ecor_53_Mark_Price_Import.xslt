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

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
  </xsl:template>

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'X'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>


  <xsl:template name="MonthCode1">
    <xsl:param name="varMonth"/>
    <xsl:choose>
      <xsl:when test="$varMonth=1">
        <xsl:value-of select="'F'"/>
      </xsl:when>
      <xsl:when test="$varMonth=2">
        <xsl:value-of select="'G'"/>
      </xsl:when>
      <xsl:when test="$varMonth=3">
        <xsl:value-of select="'H'"/>
      </xsl:when>
      <xsl:when test="$varMonth=4">
        <xsl:value-of select="'J'"/>
      </xsl:when>
      <xsl:when test="$varMonth=5">
        <xsl:value-of select="'K'"/>
      </xsl:when>
      <xsl:when test="$varMonth=6">
        <xsl:value-of select="'M'"/>
      </xsl:when>
      <xsl:when test="$varMonth=7">
        <xsl:value-of select="'N'"/>
      </xsl:when>
      <xsl:when test="$varMonth=8">
        <xsl:value-of select="'Q'"/>
      </xsl:when>
      <xsl:when test="$varMonth=9">
        <xsl:value-of select="'U'"/>
      </xsl:when>
      <xsl:when test="$varMonth=10">
        <xsl:value-of select="'V'"/>
      </xsl:when>
      <xsl:when test="$varMonth=11">
        <xsl:value-of select="'X'"/>
      </xsl:when>
      <xsl:when test="$varMonth=12">
        <xsl:value-of select="'Z'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="/">

    <DocumentElement>
      <xsl:for-each select ="//PositionMaster">


     


        <xsl:variable name="MarkPrice">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL38"/>
          </xsl:call-template>
        </xsl:variable>


        <xsl:if test="number($MarkPrice) and contains(COL6,'Fixed Income')='true'">



          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'WELLS FARGO'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL13"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="PB_SUFFIX_NAME">

              <xsl:choose>
                <xsl:when test="contains(COL27,'_')">
                  <xsl:value-of select="substring-before(COL27,'_')"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="COL27"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <xsl:variable name="PRANA_SUFFIX_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
            </xsl:variable>

            <xsl:variable name="AssetType">
              <xsl:choose>

                <xsl:when test="contains(COL6,'Options')">
                  <xsl:value-of select="'Option'"/>
                </xsl:when>

                <xsl:when test="contains(COL6,'Fixed Income')">
                  <xsl:value-of select="'Fixed Income'"/>
                </xsl:when>

                <xsl:when test="contains(COL6,'Futures')">
                  <xsl:value-of select="'Future'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PB_ROOT_NAME">

              <xsl:choose>
                <xsl:when test="string-length(substring-before(COL12,'_'))=5">
                  <xsl:value-of select="concat(substring(COL12,1,3),' ',substring(COL12,4,2))"/>
                </xsl:when>

                <xsl:when test="string-length(substring-before(COL12,'_'))=4">
                  <xsl:value-of select="concat(substring(COL12,1,2),' ',substring(COL12,3,2))"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="substring-before(COL12,'_')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PB_YELLOW_NAME">
              <!--<xsl:value-of select="normalize-space()"/>-->
              <!--<xsl:choose>
								<xsl:when test ="contains(substring-after(normalize-space(COL20),' '),' ')">
									<xsl:value-of select="substring-after(substring-after(normalize-space(COL20),' '),' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-after(normalize-space(COL20),' ')"/>
								</xsl:otherwise>
							</xsl:choose>-->
              <xsl:value-of select="normalize-space(COL41)"/>
            </xsl:variable>

            <!--<xsl:variable name="PB_EXCHANGE_NAME">
							<xsl:value-of select="normalize-space(COL91)"/>
						</xsl:variable>-->

            <xsl:variable name ="PRANA_ROOT_NAME">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME and @YellowFlag = $PB_YELLOW_NAME]/@UnderlyingCode"/>
            </xsl:variable>

            <xsl:variable name ="FUTURE_EXCHANGE_CODE">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME and @YellowFlag= $PB_YELLOW_NAME]/@ExchangeCode"/>
            </xsl:variable>

            <xsl:variable  name="FUTURE_FLAG">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME and @YellowFlag= $PB_YELLOW_NAME]/@ExpFlag"/>
            </xsl:variable>

            <xsl:variable name="MonthCode">
              <!--<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="number(substring-before(substring-after(normalize-space(COL18),'/'),'/'))"/>
							</xsl:call-template>-->
              <xsl:choose>
                <xsl:when test="string-length(substring-before(COL12,'_'))=5">
                  <xsl:value-of select="substring(COL12,4,1)"/>
                </xsl:when>

                <xsl:when test="string-length(substring-before(COL12,'_'))=4">
                  <xsl:value-of select="substring(COL12,3,1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="Year" >

              <xsl:choose>
                <xsl:when test="string-length(substring-before(COL12,'_'))=5">
                  <xsl:value-of select="substring(COL12,5,1)"/>
                </xsl:when>

                <xsl:when test="string-length(substring-before(COL12,'_'))=4">
                  <xsl:value-of select="substring(COL12,4,1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>

            </xsl:variable>

            <xsl:variable name="MonthYearCode">
              <xsl:choose>
                <xsl:when test="$FUTURE_FLAG!=''">
                  <xsl:value-of select="concat($Year,$MonthCode)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat($MonthCode,$Year)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="Underlying">
              <xsl:choose>
                <xsl:when test="$PRANA_ROOT_NAME!=''">
                  <xsl:value-of select="translate($PRANA_ROOT_NAME,$lower_CONST,$upper_CONST)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="translate($PB_ROOT_NAME,$lower_CONST,$upper_CONST)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>



            <xsl:variable name="Symbol">
              <xsl:value-of select="COL12"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>

                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="translate($PRANA_SYMBOL_NAME,$lower_CONST,$upper_CONST)"/>
                </xsl:when>


                <xsl:when test="$AssetType='Option'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$AssetType='Future'">
                  <xsl:value-of select="$PB_ROOT_NAME"/>
                </xsl:when>


                <xsl:when test="$Symbol!='*'">
                  <xsl:value-of select="concat($Symbol,$PRANA_SUFFIX_NAME)"/>
                </xsl:when>

                <!--<xsl:when test="$Symbol!='*'">
                  <xsl:value-of select="normalize-space(concat($Underlying,' ',$MonthYearCode))"/>
                </xsl:when>-->

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>

              </xsl:choose>

            </Symbol>

            <IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$AssetType='Option'">
                  <xsl:value-of select="concat(substring($Symbol,1,21),'U')"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>


            <MarkPrice>
              <xsl:choose>
                <xsl:when test="$MarkPrice &gt; 0">
                  <xsl:value-of select="$MarkPrice"/>

                </xsl:when>
                <xsl:when test="$MarkPrice &lt; 0">
                  <xsl:value-of select="$MarkPrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </MarkPrice>


            <!--<xsl:variable name="Side" select="COL1"/>


            <SideTagValue>
              <xsl:choose>

                <xsl:when test="$Side='Short'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>

                <xsl:when test="$Side='Long'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </SideTagValue>-->


            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>


            <xsl:variable name="Date" select="COL1"/>

            <Date>
              <xsl:value-of select="$Date"/>
            </Date>


            <SMRequest>
              <xsl:value-of select="'true'"/>
            </SMRequest>


          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>