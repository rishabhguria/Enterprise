<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Cash">
					<xsl:value-of select="COL10"/>
				</xsl:variable>

				<xsl:if test="number($Cash)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'WF'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL12"/>
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

						<xsl:variable name="PB_FUND_NAME" select="COL1"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<FundName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</FundName>

						<xsl:variable name="Date" select="COL2"/>

						<Date>
							<xsl:value-of select="$Date"/>
						</Date>

						<!--<xsl:variable name="PB_CURRENCY_NAME" select="COL4"/>

						<xsl:variable name="PRANA_CURRENCY_ID">
							<xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@PBCurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyID"/>
						</xsl:variable>-->


						<xsl:variable name="CurrencyName">
							<xsl:value-of select="COL13"/>
						</xsl:variable>
						<CurrencyName>
							<xsl:choose>
								<xsl:when test="contains($CurrencyName,'_')">
									<xsl:value-of select="substring-before($CurrencyName,'_')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$CurrencyName"/>
								</xsl:otherwise>
							</xsl:choose>
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

								<xsl:when test="COL6='2E'">
									<xsl:value-of select="'Interest_Expense'"/>
								</xsl:when>
								<xsl:when test="COL6='DE'">
									<xsl:value-of select="'Dividend_Expense'"/>
								</xsl:when>
								<xsl:when test="COL6='DI'">
									<xsl:value-of select="'Dividend_Income'"/>
								</xsl:when>
								<xsl:when test="COL6='FI'">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="COL6='MI'">
									<xsl:value-of select="'Intersest_Income'"/>
								</xsl:when>
								<xsl:when test="COL6='ME'">
									<xsl:value-of select="'Interest_Expense'"/>
								</xsl:when>
								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="CASH_WDL"/>
									
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="CASH_DEP"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<JournalEntries>
							<xsl:choose>
								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $AbsCash, '|Cash:',$AbsCash)"/>
								</xsl:when>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="concat( 'Cash:',$AbsCash,'|', $PRANA_ACRONYM_NAME,':' , $AbsCash)"/>
								</xsl:when>
							</xsl:choose>
						</JournalEntries>

						<!--<xsl:variable name="Description" select="COL12"/>-->

						   <Description>
							   <!--<xsl:value-of select="COL12"/>-->
							   <!--<xsl:value-of select="$Description"/>-->
							   <xsl:choose>
								   <xsl:when test="COL12!='*'" >
									   <xsl:value-of select="normalize-space(COL12)"/>
								   </xsl:when>
								   <xsl:otherwise>
									   <xsl:value-of select="''"/>
								   </xsl:otherwise>
							   </xsl:choose>
                            </Description> 

					</PositionMaster> 
				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>