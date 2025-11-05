<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>

      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'True'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

        <ACC>
          <xsl:value-of select="'ACC'"/>
        </ACC>

        <TYP>
          <xsl:value-of select="'TYP'"/>
        </TYP>

        <BS>
          <xsl:value-of select="'B/S'"/>
        </BS>

        <SYM>
          <xsl:value-of select="'SYM'"/>
        </SYM>

        <QTY>
          <xsl:value-of select="'QTY'"/>
        </QTY>

        <PRC>
          <xsl:value-of select="'PRC'"/>
        </PRC>

        <COMM>
          <xsl:value-of select="'COMM'"/>
        </COMM>

        <TRDDT>
          <xsl:value-of select="'TRD DT'"/>
        </TRDDT>

        <SETDT>
          <xsl:value-of select="'SET DT'"/>
        </SETDT>

        <EXEC>
          <xsl:value-of select="'EXEC'"/>
        </EXEC>

        <STREET>
          <xsl:value-of select="'STREET'"/>
        </STREET>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">


        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

          <ACC>
            <xsl:value-of select="AccountNo"/>
          </ACC>

          <TYP>
            <xsl:choose>
              <xsl:when test="CompanyAccountType='Margin'">
                <xsl:value-of select="'2'"/>
              </xsl:when>
              <xsl:when test="CompanyAccountType='Cash'">
                <xsl:value-of select="'1'"/>
              </xsl:when>
              <xsl:when test="CompanyAccountType='Short'">
                <xsl:value-of select="'5'"/>
              </xsl:when>
            </xsl:choose>
          </TYP>

          <BS>
            <xsl:value-of select="Side"/>
          </BS>

          <SYM>
            <xsl:value-of select="Symbol"/>
          </SYM>

          <QTY>
            <xsl:value-of select="AllocationQty"/>
          </QTY>

          <PRC>
            <xsl:value-of select="AveragePrice"/>
          </PRC>

          <COMM>
            <xsl:value-of select="CommissionCharged"/>
          </COMM>

          <TRDDT>
            <xsl:value-of select="TradeDate"/>
          </TRDDT>

          <SETDT>
            <xsl:value-of select="'SettlementDate'"/>
          </SETDT>

          <EXEC>
            <xsl:value-of select="''"/>
          </EXEC>

          <STREET>
            <xsl:value-of select="CounterParty"/>
          </STREET>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

</xsl:stylesheet>