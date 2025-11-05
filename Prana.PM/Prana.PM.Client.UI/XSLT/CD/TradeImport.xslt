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
        <xsl:if test ="number(COL2)">

          <PositionMaster>
            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='MSPB']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
            </xsl:choose>

           

            <PositionStartDate>
              <xsl:value-of select="COL1"/>
            </PositionStartDate>

			  <xsl:variable name = "PB_COMPANY" >
				  <xsl:value-of select="COL5"/>
			  </xsl:variable>
			  <xsl:variable name="PRANA_SYMBOL">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='MSPB']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
			  </xsl:variable>

            <Symbol>
				<xsl:choose>
					<xsl:when test ="$PRANA_SYMBOL != ''">
						<xsl:value-of select ="$PRANA_SYMBOL"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test ="contains(COL4,'.cn') != false">
								<xsl:value-of select ="translate(concat(substring-before(COL4,'.'),'-TC'),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="translate(translate(COL4,',',''),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</Symbol>

			  <FXConversionMethodOperator>
				  <xsl:value-of select ="'M'"/>
			  </FXConversionMethodOperator>

			  <FXRate>
				  <xsl:value-of select ="1"/>
			  </FXRate>

            <PBSymbol>
              <xsl:value-of select="COL5"/>
            </PBSymbol>

			  <!--<Description>
				  <xsl:choose>
					  <xsl:when test ="boolean(number(COL13))">
						  <xsl:value-of select="COL13"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Description>-->

			  <Description>
				  <xsl:value-of select="COL13"/>
			  </Description>

			  <!--QUANTITY-->

			  <NetPosition>
				  <xsl:choose>
					  <xsl:when test="COL2 &lt; 0">
                  <xsl:value-of select="COL2 * (-1)"/>
                </xsl:when>
                <xsl:when test="COL2 &gt; 0">
                  <xsl:value-of select="COL2"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <!--Side-->

            <SideTagValue>
				<xsl:choose>
					<xsl:when test="COL2 &lt; 0">
						<xsl:value-of select="'5'"/>
					</xsl:when>
					<xsl:when test="COL2 &gt; 0">
						<xsl:value-of select="'1'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
            </SideTagValue>

			  <Strategy>
				  <!--<xsl:choose>
					  <xsl:when test ="COL6 = 'CB (R/R) Lt. 1'">
						  <xsl:value-of select ="'CB (R/R) Lt. 1'"/>
					  </xsl:when>
					  <xsl:when test ="COL6 = 'PS (R/R) Lt 1'">
						  <xsl:value-of select ="'PS (R/R) Lt 1'"/>
					  </xsl:when>
					  <xsl:when test ="COL6 = 'Rest/Reg Lot 1'">
						  <xsl:value-of select ="'Rest/Reg Lot 1'"/>
					  </xsl:when>
					  <xsl:when test ="COL6 = 'Rest/Reg Lot 2'">
						  <xsl:value-of select ="'Rest/Reg Lot 2'"/>
					  </xsl:when>
					  <xsl:when test ="COL6 = 'Rest/Reg Lot 2'">
						  <xsl:value-of select ="'Rest/Reg Lot 2'"/>
					  </xsl:when>
					  <xsl:when test ="COL6 = 'Rest/Reg Lot 3'">
						  <xsl:value-of select ="'Rest/Reg Lot 3'"/>
					  </xsl:when>
					  <xsl:when test ="COL6 = 'Rest/Reg Lot 4'">
						  <xsl:value-of select ="'Rest/Reg Lot 4'"/>
					  </xsl:when>
					  <xsl:when test ="COL6 = 'Rule 144'">
						  <xsl:value-of select ="'Rule 144'"/>
					  </xsl:when>
					  <xsl:when test ="COL6 = 'Sell in Canada only - Reg S'">
						  <xsl:value-of select ="'Sell in Canada only - Reg S'"/>
					  </xsl:when>
					  <xsl:when test ="COL6 = 'Rest/Reg Lot 4'">
						  <xsl:value-of select ="'Rest/Reg Lot 4'"/>
					  </xsl:when>
					  
					  
					  <xsl:when test ="COL6 = 'Wts (R/R) Lt 1'">
						  <xsl:value-of select ="'Wts (R/R) Lt 1'"/>
					  </xsl:when>
					  <xsl:when test ="COL6 = 'Wts (R/R) Lt 2'">
						  <xsl:value-of select ="'Wts (R/R) Lt 2'"/>
					  </xsl:when>
					  <xsl:when test ="COL6 = 'Rest/Reg Lot 2'">
						  <xsl:value-of select ="'Rest/Reg Lot 2'"/>
					  </xsl:when>
				  </xsl:choose>-->
				  <xsl:value-of select ="COL6"/>
			  </Strategy>

			  <CostBasis>
				  <xsl:choose>
					  <xsl:when test ="boolean(number(COL7))">
						  <xsl:value-of select="COL7"/>
					  </xsl:when>
					  <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

			  <!--<CommissionSource>
				  <xsl:value-of select ="'Manual'"/>
			  </CommissionSource>-->
            
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
