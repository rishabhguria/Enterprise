<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
	
	<DocumentElement>

		<xsl:for-each select ="//PositionMaster">

      <xsl:variable name="Position" >
        <!--<xsl:choose>
          <xsl:when test="COL7='EQT'">
            <xsl:value-of select="COL102"/>
          </xsl:when>
          <xsl:otherwise>-->
            <xsl:value-of select="COL104"/>
          <!--</xsl:otherwise>
        </xsl:choose>-->
      </xsl:variable>

			<xsl:if test="number($Position) and normalize-space(COL7)!='FIXED INCOME'">

				<PositionMaster>

					<xsl:variable name="PB_NAME">
						<xsl:value-of select="'Statestreet'"/>
					</xsl:variable>

					<xsl:variable name = "PB_SYMBOL_NAME" >
            <xsl:choose>
              <xsl:when test="COL7='EQT'">
                <xsl:value-of select="COL92"/>
              </xsl:when>
              <xsl:when test="COL90='USD2437907-S'">
                <xsl:value-of select="'QIAGEN NV'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="normalize-space(COL12)"/>
              </xsl:otherwise>
            </xsl:choose>
						
					</xsl:variable>

					<xsl:variable name="PRANA_SYMBOL_NAME">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
					</xsl:variable>
					
					<xsl:variable name="apos">'</xsl:variable>
					
          <xsl:variable name="VarSymbol">
            <xsl:choose>
              <xsl:when test="COL7='EQT'">
                <xsl:value-of select="substring-before(substring(substring-after(COL90,$apos),4),'-')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
              </xsl:choose>
          </xsl:variable>

					<Symbol>
						<xsl:choose>

							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
							</xsl:when>

							<xsl:when test="$VarSymbol!='*'">
								<xsl:value-of select="$VarSymbol"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</xsl:otherwise>

						</xsl:choose>
					</Symbol>
					
					<!--<xsl:variable name="Cusip" select="substring(COL11,2)"/>

					<CUSIP>
						<xsl:choose>

							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<xsl:value-of select="''"/>
							</xsl:when>

							<xsl:when test="$Cusip!=''">
								<xsl:value-of select="$Cusip"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>

						</xsl:choose>
					</CUSIP>-->

					<!--<xsl:variable name="PB_FUND_NAME" select="COL1"/>

					<xsl:variable name ="PRANA_FUND_NAME">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
					</AccountName>-->

					<!--<SideTagValue>
						<xsl:choose>

              <xsl:when test="COL7='EQT'">
                <xsl:choose>
                  <xsl:when test="COL94 ='Long'">
                    <xsl:value-of select="'1'"/>
                  </xsl:when>

                  <xsl:when test="COL94 ='Short'">
                    <xsl:value-of select="'5'"/>
                  </xsl:when>

                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="$Position &gt; 0">
                    <xsl:value-of select="'1'"/>
                  </xsl:when>

                  <xsl:when test="$Position &lt; 0">
                    <xsl:value-of select="'5'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="'0'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>

						

						</xsl:choose>
					</SideTagValue>-->

					<MarkPrice>
						<xsl:choose>

							<xsl:when test="$Position &gt; 0">
								<xsl:value-of select="$Position"/>
							</xsl:when>

							<xsl:when test="$Position &lt; 0">
								<xsl:value-of select="$Position * (-1)"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>

						</xsl:choose>
					</MarkPrice>

						<xsl:variable name="Cost" >
            <xsl:choose>
              <xsl:when test="COL7='EQT'">
                <xsl:value-of select="number(COL104)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="number(COL22)"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>




					<Date>
						<xsl:value-of select="COL4"/>
					</Date>

						<PBSymbol>
						<xsl:value-of select ="$PB_SYMBOL_NAME"/>
					</PBSymbol>


				</PositionMaster>

			</xsl:if>

		</xsl:for-each>

	</DocumentElement>
	
</xsl:template>

</xsl:stylesheet>