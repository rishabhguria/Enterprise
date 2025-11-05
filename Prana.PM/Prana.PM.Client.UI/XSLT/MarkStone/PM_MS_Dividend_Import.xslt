<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

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

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="varDividend">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL9"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varDividend) and contains(COL1,'dv')='true'">

          <PositionMaster>

            <xsl:variable name="PB_Name">
              <xsl:value-of select="'MarkStone'"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/AccountMapping/PB[@Name = $PB_Name]/AccountData[@PBAccountCode=$PB_FUND_NAME]/@PranaAccount"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol" select="COL2"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>


            <!--<xsl:variable name="PB_SUFFIX_NAME">
              <xsl:value-of select="normalize-space(COL3)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SUFFIX_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_Name]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
            </xsl:variable>-->

            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME = ''">
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <xsl:variable name="Symbol" >
              <xsl:value-of select="''"/>
            </xsl:variable>

            <Symbol>
				<xsl:choose>
				<xsl:when test="$PRANA_SYMBOL_NAME!=''">
					<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
				</xsl:when>

				<xsl:otherwise>
					<xsl:value-of select="$PB_Symbol"/>
				</xsl:otherwise>
				</xsl:choose>
            </Symbol>



            <Amount>
              <xsl:choose>
                <xsl:when test="number($varDividend)">
                  <xsl:value-of select="$varDividend"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Amount>


            <xsl:variable name ="Date" select="COL3"/>


            <!--<xsl:variable name="Year1" select="substring($Date,1,4)"/>
            <xsl:variable name="Month" select="substring($Date,5,2)"/>
            <xsl:variable name="Day" select="substring($Date,7,2)"/>-->




            <PayoutDate>
              <!--<xsl:value-of select="concat($Month,'/',$Day,'/',$Year1)"/>-->
              <xsl:value-of select="$Date"/>

            </PayoutDate>

            <xsl:variable name ="Date1" select="COL4"/>


            <!--<xsl:variable name="Year" select="substring($Date1,1,4)"/>
            <xsl:variable name="Month1" select="substring($Date1,5,2)"/>
            <xsl:variable name="Day1" select="substring($Date1,7,2)"/>-->




            <ExDate>
              <!--<xsl:value-of select="concat($Month1,'/',$Day1,'/',$Year)"/>-->

              <xsl:value-of select="$Date1"/>
            </ExDate>

            <RecordDate>

              <xsl:value-of select="''"/>
            </RecordDate>


            <Currency>
              <xsl:value-of select="''"/>
            </Currency>



			  <Description>
				  <xsl:choose>

					  <xsl:when test="$varDividend &gt; 0">
						  <xsl:value-of select="'Dividend Received'"/>
					  </xsl:when>
					  <xsl:when test ="$varDividend &lt; 0">
						  <xsl:value-of select ="'Dividend Charged'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>

				  </xsl:choose>
			  </Description>


            <!--<PBSymbol>
              <xsl:value-of select="$PB_Symbol"/>
            </PBSymbol>-->

            <ActivityType>
              <xsl:choose>

                <xsl:when test="$varDividend &gt; 0">
                  <xsl:value-of select="'DividendIncome'"/>
                </xsl:when>
                <xsl:when test ="$varDividend &lt; 0">
                  <xsl:value-of select ="'DividendExpense'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </ActivityType>

          </PositionMaster>
        </xsl:if>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>