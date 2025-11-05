<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test ="number(COL7)">

          <PositionMaster>
            <!--   Fund -->
            <!--fundname section-->
            <!--<xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL5"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Knight']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>-->

			  <xsl:variable name="PB_Symbol" select="COL6"/>
			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Knight']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
			  </xsl:variable>

            <!--<xsl:choose>
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
            </xsl:choose>-->

			  <AccountName>
				  <xsl:value-of select="''"/>
			  </AccountName>
			  
            <PositionStartDate>
              <xsl:value-of select="COL3"/>
            </PositionStartDate>

            <xsl:variable name="varUnderlyingLength">
              <xsl:value-of select="string-length(substring-before(COL15,' '))"/>
            </xsl:variable>

            <xsl:variable name="varSpace">
              <xsl:call-template name="noofBlanks" >
                <xsl:with-param name="count1" select="6-($varUnderlyingLength)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varOSI">
              <xsl:value-of select="concat(substring-before(COL15, ' '),$varSpace, substring-after(COL15, ' '))"/>
            </xsl:variable>

            <Symbol>
				<xsl:choose>
					<xsl:when test ="$PRANA_SYMBOL_NAME = ''">
						<xsl:value-of select="substring-before(COL6,' ')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
					</xsl:otherwise>
				</xsl:choose>

			</Symbol>

            <!--<Symbol>
              <xsl:choose>
                <xsl:when test="normalize-space(COL4)='S'">
                  <xsl:value-of select="COL12"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varThirdFriday"/>
                  -->
            <!--<xsl:choose>
                    <xsl:when test="varIsFlex = 0">
                    <xsl:value-of select="concat('O:',normalize-space(COL12),' ',substring(COL1,3,2),$MonthCode,format-number(COL3,'#.00'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="concat('O:',normalize-space(COL12),' ',substring(COL1,3,2),$MonthCode,format-number(COL3,'#.00'),'D',$varDate)"/>
                    </xsl:otherwise>
                  </xsl:choose>-->
            <!--
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>-->

            <PBSymbol>
              <xsl:value-of select="COL6"/>
            </PBSymbol>

            <!--<PBAssetType>
              <xsl:value-of select ="COL5"/>
            </PBAssetType>-->

            <!--QUANTITY-->


            <NetPosition>
              <xsl:choose>
                <xsl:when test="COL7 &lt; 0">
                  <xsl:value-of select="COL7 * (-1)"/>
                </xsl:when>
                <xsl:when test="COL7 &gt; 0">
                  <xsl:value-of select="COL7"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <!--Side-->

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="normalize-space(COL12) = 'B'">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <xsl:when test="normalize-space(COL12) = 'S'">
                  <xsl:value-of select="2"/>
                </xsl:when>
                <xsl:when test="normalize-space(COL12) = 'SS'">
                  <xsl:value-of select="5"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <xsl:choose>
              <xsl:when test ="boolean(number(COL8))">
                <CostBasis>
                  <xsl:value-of select="COL8"/>
                </CostBasis>
              </xsl:when>
              <xsl:otherwise>
                <CostBasis>
                  <xsl:value-of select="0"/>
                </CostBasis>
              </xsl:otherwise>
            </xsl:choose>

            <Commission>
              <xsl:choose>
                <xsl:when test="COL9 &gt; 0">
                  <xsl:value-of select="COL9"/>
                </xsl:when>
                <xsl:when test="COL9 &lt; 0">
                  <xsl:value-of select="COL9*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <!--<TransactionLevy>
              <xsl:choose>
                <xsl:when test ="boolean(number(COL10))">
                  <xsl:value-of select="COL10"/>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </TransactionLevy>-->

            <xsl:variable name="varMiscFee">
              <xsl:value-of select="COL13 + COL14 + COL15"/>
            </xsl:variable>

            <MiscFees>
              <xsl:choose>
                <xsl:when test ="number($varMiscFee)">
					<xsl:choose>
						<xsl:when test="number($varMiscFee) &gt; 0">
							<xsl:value-of select="$varMiscFee"/>
						</xsl:when>
						<xsl:when test="number($varMiscFee) &lt; 0">
							<xsl:value-of select="number($varMiscFee) * (-1)"/>
						</xsl:when>
					</xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MiscFees>

            <Fees>
              <xsl:choose>
                <xsl:when test ="number(COL11)">
					<xsl:choose>
						<xsl:when test="number(COL11) &gt; 0">
							<xsl:value-of select="COL11"/>
						</xsl:when>
						<xsl:when test="number(COL11) &lt; 0">
							<xsl:value-of select="number(COL11) * (-1)"/>
						</xsl:when>
					</xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
