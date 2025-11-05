<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Cash">
          <xsl:value-of select="COL9"/>
        </xsl:variable>

        <xsl:if test="number($Cash) and  contains(COL1,'dp')='true' or contains(COL1,'wd')='true'">

			<PositionMaster>

				<xsl:variable name="PB_NAME">
					<xsl:value-of select="'MS'"/>
				</xsl:variable>

				<xsl:variable name = "PB_SYMBOL_NAME" >
					<xsl:value-of select ="''"/>
				</xsl:variable>

				<xsl:variable name="PRANA_SYMBOL_NAME">
					<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
				</xsl:variable>

				<!--<Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>-->

				<xsl:variable name="PB_FUND_NAME" select="COL11"/>

				<xsl:variable name ="PRANA_FUND_NAME">
					<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/AccountMapping/PB[@Name=$PB_NAME]/AccountData[@PBAccountName=$PB_FUND_NAME]/@PranaAccount"/>
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

				<xsl:variable name="Date" select="COL3"/>

				<Date>
					<xsl:value-of select="$Date"/>
				</Date>

				<!--<xsl:variable name="PB_CURRENCY_NAME" select="COL4"/>

						<xsl:variable name="PRANA_CURRENCY_ID">
							<xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@PBCurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyID"/>
						</xsl:variable>-->

				<CurrencyName>
					<xsl:value-of select="'USD'"/>
				</CurrencyName>

				<CurrencyID>
					<!--<xsl:choose>
								<xsl:when test="$PRANA_CURRENCY_ID!=''">
									<xsl:value-of select="$PRANA_CURRENCY_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose>-->
					<xsl:value-of select="1"/>
				</CurrencyID>

				<xsl:variable name="AbsCash">
					<xsl:choose>
						<xsl:when test="$Cash &gt; 0">
							<xsl:value-of select="$Cash"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$Cash * -1"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="PRANA_ACRONYM_NAME">
					<xsl:choose>

						<xsl:when test="contains(COL1,'wd')">
							<xsl:value-of select="'CASH_WDL'"/>
						</xsl:when>


						<xsl:when test="COL1='dp'">
							<xsl:value-of select="'CASH_DEP'"/>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>


				<JournalEntries>
					<xsl:choose>
						<xsl:when test="COL1='wd'">
							<xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $Cash, '|Cash:',$Cash)"/>
						</xsl:when>
						<xsl:when test="COL1='dp'">
							<xsl:value-of select="concat( 'Cash:',$Cash,'|', $PRANA_ACRONYM_NAME,':' , $Cash)"/>
						</xsl:when>
					</xsl:choose>
				</JournalEntries>
				<xsl:variable name="Description" select="COL2"/>

				<!--<Description>
              <xsl:value-of select="$Description"/>
            </Description>-->

			</PositionMaster>
        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>